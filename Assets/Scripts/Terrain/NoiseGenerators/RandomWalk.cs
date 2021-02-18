using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Terrain.NoiseGenerators
{
    public class RandomWalkTerrainGenerator : MonoBehaviour
    {
        public static int[,] RandomWalkTop(int[,] map, float seed)
        {
            //Seed our random
            System.Random rand = new System.Random(seed.GetHashCode());

            //Set our starting height
            int lastHeight = Random.Range(0, map.GetUpperBound(1));

            //Cycle through our width
            for (int x = 0; x < map.GetUpperBound(0); x++)
            {
                //Flip a coin
                int nextMove = rand.Next(2);

                //If heads, and we aren't near the bottom, minus some height
                if (nextMove == 0 && lastHeight > 2)
                {
                    lastHeight--;
                }
                //If tails, and we aren't near the top, add some height
                else if (nextMove == 1 && lastHeight < map.GetUpperBound(1) - 2)
                {
                    lastHeight++;
                }

                //Circle through from the lastheight to the bottom
                for (int y = lastHeight; y >= 0; y--)
                {
                    map[x, y] = 1;
                }
            }
            //Return the map
            return map;
        }

        public static int[,] RandomWalkTopSmoothed(int[,] map, float seed, int minSectionWidth)
        {
            //Seed our random
            System.Random rand = new System.Random(seed.GetHashCode());

            //Determine the start position
            int lastHeight = Random.Range(0, map.GetUpperBound(1));

            //Used to determine which direction to go
            int nextMove = 0;
            //Used to keep track of the current sections width
            int sectionWidth = 0;

            //Work through the array width
            for (int x = 0; x <= map.GetUpperBound(0); x++)
            {
                //Determine the next move
                nextMove = rand.Next(2);

                //Only change the height if we have used the current height more than the minimum required section width
                if (nextMove == 0 && lastHeight > 0 && sectionWidth > minSectionWidth)
                {
                    lastHeight--;
                    sectionWidth = 0;
                }
                else if (nextMove == 1 && lastHeight < map.GetUpperBound(1) && sectionWidth > minSectionWidth)
                {
                    lastHeight++;
                    sectionWidth = 0;
                }
                //Increment the section width
                sectionWidth++;

                //Work our way from the height down to 0
                for (int y = lastHeight; y >= 0; y--)
                {
                    map[x, y] = 1;
                }
            }

            //Return the modified map
            return map;
        }
    }
}