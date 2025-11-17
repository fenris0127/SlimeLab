namespace SlimeLab.Systems
{
    public class GridCell
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public TerrainType TerrainType { get; private set; }

        public GridCell(int x, int y)
        {
            X = x;
            Y = y;
            TerrainType = TerrainType.Normal;
        }

        public void SetTerrainType(TerrainType terrainType)
        {
            TerrainType = terrainType;
        }
    }

    public class GridMap
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        private GridCell[,] _cells;

        public GridMap(int width, int height)
        {
            Width = width;
            Height = height;
            _cells = new GridCell[width, height];

            // Initialize all cells
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    _cells[x, y] = new GridCell(x, y);
                }
            }
        }

        public GridCell GetCell(int x, int y)
        {
            // Check bounds
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                return null;
            }

            return _cells[x, y];
        }
    }
}
