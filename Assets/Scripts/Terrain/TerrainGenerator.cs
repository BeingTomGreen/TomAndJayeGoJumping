using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Terrain
{
    public class TerrainGenerator : MonoBehaviour
    {
        [Header("Map Generation:")]
        [Range(0, 100)]
        public int FillPercentage;
        public int Width;
        public int Height;
        public int SmoothingPasses;

        [Header("Map Seed:")]
        public string Seed;
        public bool UseRandomSeed;

        [Header("Debug:")]
        public bool DrawGizmos;

        protected int[,] map;

        [Header("Map Decoration:")]
        public Tilemap TerrainTilemap;
        public TileBase TerrainTile;

        protected void GenerateMap()
        {
            map = new int[Width, Height];
        }

        protected void RandomFillMap()
        {
            if (UseRandomSeed || Seed.Length == 0)
            {
                Seed = Time.time.ToString();
            }

            // Grab a sudo random seed using our supplied (or generated seed)
            System.Random pseudoRandomSeed = new System.Random(Seed.GetHashCode());

            // Loop our x - IE the map width
            for (int x = 0; x < Width; x++)
            {
                // Loop our y - IE the map height
                for (int y = 0; y < Height; y++)
                {
                    // Check if this is a border tile
                    if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
                    {
                        // Force fill the borders
                        map[x, y] = 1;
                    }
                    else
                    {
                        // Generate a number beween 0-100, if less than fill percentage it should be a solid tile, more than and it should be empty
                        map[x, y] = pseudoRandomSeed.Next(0, 100) < FillPercentage ? 1 : 0;
                    }
                }
            }
        }

        protected void SmoothMap(int SmoothingPasses)
        {
            for (int i = 0; i < SmoothingPasses; i++)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        int neighbourWallTIles = GetSurroundingWallCount(x, y);

                        // If more than 4 neighbouring tiles, make this tile a wall
                        if (neighbourWallTIles > 4)
                        {
                            map[x, y] = 1;
                        }
                        // Less than 4 neighbours = empty tile
                        else if (neighbourWallTIles < 4)
                        {
                            map[x, y] = 0;
                        }
                    }
                }
            }
        }

        /*
         * Calulates the number of filled tiles around a specified tile
         * 
         */
        protected int GetSurroundingWallCount(int gridX, int gridY)
        {
            int wallCount = 0;

            // Loop through a 3 x 3 grid around the specified tile
            for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
            {
                for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
                {
                    // Check the tile we're checking isn't out of bounds
                    // Basically handles checking an edge tile where the x/y could be 0, then -1 above would cause errors
                    if (neighbourX >= 0 && neighbourX < Width && neighbourY >= 0 && neighbourY < Height)
                    {
                        // Check that we're not on the tile we're searching around
                        if (neighbourX != gridX || neighbourY != gridY)
                        {
                            // Since the tile will be 1 if populated, or 0 if not we can use use map[x,y] to increment (or not) wall count
                            wallCount += map[neighbourX, neighbourY];
                        }
                    }
                    else
                    {
                        wallCount++;
                    }
                }
            }

            return wallCount;
        }

        public void RenderMap(int[,] map, Tilemap tilemap, TileBase tile)
        {
            //Clear the map (ensures we dont overlap)
            tilemap.ClearAllTiles();

            //Loop through the width of the map
            for (int x = 0; x < Width; x++)
            {
                //Loop through the height of the map
                for (int y = 0; y < Height; y++)
                {
                    // 1 = tile, 0 = no tile
                    if (map[x, y] == 1)
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                    }
                }
            }

            tilemap.CompressBounds();
        }

        // Hack to visualise our map
        protected void OnDrawGizmos()
        {
            if (map != null && DrawGizmos == true)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
                        Vector3 pos = new Vector3(-Width / 2 + x + .5f, -Height / 2 + y + .5f, 0);
                        Gizmos.DrawCube(pos, Vector3.one);
                    }
                }
            }
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Generate();
        }

        public void Generate()
        {
            GenerateMap();
            RandomFillMap();
            //SmoothMap(SmoothingPasses);
            RenderMap(map, TerrainTilemap, TerrainTile);
        }
    }
}

