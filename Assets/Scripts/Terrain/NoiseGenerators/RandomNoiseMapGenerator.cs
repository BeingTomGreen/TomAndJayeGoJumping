using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Terrain.NoiseGenerators
{
    public class RandomNoiseMapGenerator : NoiseMapGenerator
    {
        public string Seed;
        public bool UseRandomSeed;
        public int FillPercentage;

        public override NoiseMap GenerateNoiseMap(int width, int height)
        {
            NoiseMap noiseMap = new NoiseMap(width, height);

            noiseMap = RandomFillMap(ref noiseMap);

            return noiseMap;
        }

        protected NoiseMap RandomFillMap(ref NoiseMap noiseMap)
        {
            // Do we need  random seed?
            if (UseRandomSeed || Seed.Length == 0)
            {
                Seed = Time.time.ToString();
            }

            // Grab a sudo random seed using our supplied (or generated seed)
            System.Random pseudoRandomSeed = new System.Random(Seed.GetHashCode());

            // Loop our x - IE the map width
            for (int x = 0; x < noiseMap.Width; x++)
            {
                // Loop our y - IE the map height
                for (int y = 0; y < noiseMap.Height; y++)
                {
                    // Check if this is a border tile
                    if (x == 0 || x == noiseMap.Width - 1 || y == 0 || y == noiseMap.Height - 1)
                    {
                        // Force fill the borders
                        noiseMap.map[x, y] = 1;
                    }
                    else
                    {
                        // Generate a number beween 0-100, if less than fill percentage it should be a solid tile, more than and it should be empty
                        noiseMap.map[x, y] = pseudoRandomSeed.Next(0, 100) < FillPercentage ? 1 : 0;
                    }
                }
            }

            return noiseMap;
        }

        protected void SmoothMap(NoiseMap noiseMap, int SmoothingPasses)
        {
            for (int i = 0; i < SmoothingPasses; i++)
            {
                for (int x = 0; x < noiseMap.Width; x++)
                {
                    for (int y = 0; y < noiseMap.Height; y++)
                    {
                        int neighbourWallTIles = GetSurroundingWallCount(x, y);

                        // If more than 4 neighbouring tiles, make this tile a wall
                        if (neighbourWallTIles > 4)
                        {
                            noiseMap.map[x, y] = 1;
                        }
                        // Less than 4 neighbours = empty tile
                        else if (neighbourWallTIles < 4)
                        {
                            noiseMap.map[x, y] = 0;
                        }
                    }
                }
            }
        }

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
    }
}
