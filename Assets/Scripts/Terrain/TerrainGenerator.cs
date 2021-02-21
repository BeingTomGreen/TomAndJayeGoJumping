using Assets.Scripts.Terrain.NoiseGenerators;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Terrain
{
    [RequireComponent(typeof(NoiseMapGenerator))]
    public class TerrainGenerator : MonoBehaviour
    {
        [Header("Map Constraints:")]
        public int Width;
        public int Height;

        [Header("Debug:")]
        public bool DrawGizmos;

        [Header("Map Decoration:")]
        public Tilemap TerrainTilemap;
        public TileBase TerrainTile;

        public NoiseMapGenerator noiseMapGenerator;
        protected NoiseMap noiseMap;


        [Header("Map Generation:")]
        [Range(0, 100)]
        public int FillPercentage;
        public int SmoothingPasses;

        [Header("Map Seed:")]
        public string Seed;
        public bool UseRandomSeed;








        public void RenderMap(NoiseMap noiseMap, Tilemap tilemap, TileBase tile)
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
                    if (noiseMap.map[x, y] == 1)
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                    }
                }
            }

            tilemap.CompressBounds();
        }


        public void GenerateTerrain()
        {
            noiseMap = noiseMapGenerator.GenerateNoiseMap(Width, Height);
            RenderMap(noiseMap, TerrainTilemap, TerrainTile);
        }

        /// <summary>
        /// Generate a 2 dimensional int array representing our map.
        /// </summary>
        /// <param name="force">Allow regenerating a map if one already exists.</param>
        protected void GenerateMap(bool force = false)
        {
            if (noiseMap.map == null || force == true)
            {
                noiseMap = new int[Width, Height];
            }
        }

        /// <summary>
        /// Draw our generated map as a Gizmo in the Unity Editor.
        /// </summary>
        private void OnDrawGizmos()
        {
            if (noiseMap != null && DrawGizmos == true)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        Gizmos.color = (noiseMap[x, y] == 1) ? Color.black : Color.white;
                        Vector3 pos = new Vector3(-Width / 2 + x + .5f, -Height / 2 + y + .5f, 0);
                        Gizmos.DrawCube(pos, Vector3.one);
                    }
                }
            }
        }

        /// <summary>
        /// When hook into the SceneLoaded Event when Enabled.
        /// </summary>
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        /// <summary>
        /// Handle Scene load.
        /// </summary>
        /// <param name="scene">The scene we've loaded.</param>
        /// <param name="mode">I have no idea.</param>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            GenerateTerrain();
        }
    }
}

