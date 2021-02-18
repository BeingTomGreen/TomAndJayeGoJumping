using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Terrain
{
    public class PerlinNoiseTerrainGenerator : TerrainGeneratorOld
    {
        public static int[,] PerlinNoise(int[,] map, float seed)
        {
            int newPoint;
            //Used to reduced the position of the Perlin point
            float reduction = 0.5f;
            //Create the Perlin
            for (int x = 0; x < map.GetUpperBound(0); x++)
            {
                newPoint = Mathf.FloorToInt((Mathf.PerlinNoise(x, seed) - reduction) * map.GetUpperBound(1));

                //Make sure the noise starts near the halfway point of the height
                newPoint += (map.GetUpperBound(1) / 2);
                for (int y = newPoint; y >= 0; y--)
                {
                    map[x, y] = 1;
                }
            }
            return map;
        }

        public static int[,] PerlinNoiseSmooth(int[,] map, float seed, int interval)
        {
            //Smooth the noise and store it in the int array
            if (interval > 1)
            {
                int newPoint, points;
                //Used to reduced the position of the Perlin point
                float reduction = 0.5f;

                //Used in the smoothing process
                Vector2Int currentPos, lastPos;
                //The corresponding points of the smoothing. One list for x and one for y
                List<int> noiseX = new List<int>();
                List<int> noiseY = new List<int>();

                //Generate the noise
                for (int x = 0; x < map.GetUpperBound(0); x += interval)
                {
                    newPoint = Mathf.FloorToInt((Mathf.PerlinNoise(x, (seed * reduction))) * map.GetUpperBound(1));
                    noiseY.Add(newPoint);
                    noiseX.Add(x);
                }

                points = noiseY.Count;

                //Start at 1 so we have a previous position already
                for (int i = 1; i < points; i++)
                {
                    //Get the current position
                    currentPos = new Vector2Int(noiseX[i], noiseY[i]);
                    //Also get the last position
                    lastPos = new Vector2Int(noiseX[i - 1], noiseY[i - 1]);

                    //Find the difference between the two
                    Vector2 diff = currentPos - lastPos;

                    //Set up what the height change value will be 
                    float heightChange = diff.y / interval;
                    //Determine the current height
                    float currHeight = lastPos.y;

                    //Work our way through from the last x to the current x
                    for (int x = lastPos.x; x < currentPos.x; x++)
                    {
                        for (int y = Mathf.FloorToInt(currHeight); y > 0; y--)
                        {
                            map[x, y] = 1;
                        }
                        currHeight += heightChange;
                    }
                }
            }

            else
            {
                //Defaults to a normal Perlin gen
                map = PerlinNoise(map, seed);
            }

            return map;

        }
    }
}

