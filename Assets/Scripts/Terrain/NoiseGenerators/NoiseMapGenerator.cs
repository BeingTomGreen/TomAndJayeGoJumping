namespace Assets.Scripts.Terrain.NoiseGenerators
{
    public abstract class NoiseMapGenerator
    {
        public abstract NoiseMap GenerateNoiseMap(int width, int height);
    }
}