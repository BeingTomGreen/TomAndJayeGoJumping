namespace Assets.Scripts.Terrain.NoiseGenerators
{
    public struct NoiseMap
    {
        public int Width;
        public int Height;

        public int[,] map;

        public NoiseMap(int width, int height)
        {
            Width = width;
            Height = height;

            map = new int[Width, Height];
        }
    }
}