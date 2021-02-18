using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Terrain.NoiseGenerators
{
    public interface NoiseGenerator
    {
        int[,] Generate();

    }
}