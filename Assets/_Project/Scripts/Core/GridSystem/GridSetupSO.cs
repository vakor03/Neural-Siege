using UnityEngine;

namespace _Project.Scripts.Core.GridSystem
{
    [CreateAssetMenu(menuName = "Create GridSetup", fileName = "GridSetup", order = 0)]
    public class GridSetupSO : ScriptableObject
    {
        public Array2D gridTiles;
        public int gridHeight = 10;
        public int gridWidth = 10;
        public float cellSize = 1f;
        public Vector3 originPosition = Vector3.zero;
        public bool debug;

        public void Save(Grid<Tile> grid)
        {
            gridTiles.Initialize(grid.Width, grid.Height);
            gridWidth = grid.Width;
            gridHeight = grid.Height;
            cellSize = grid.CellSize;
            originPosition = grid.OriginPosition;

            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    gridTiles[x, y] = grid[x, y] != null;
                }
            }
            gridTiles.isInitialized = true;
        }

        public Grid<Tile> Load(Tile tilePrefab, Transform gridTransform)
        {
            Grid<Tile> grid = new(gridWidth, gridHeight, cellSize, originPosition, debug);

            if (!gridTiles.isInitialized)
            {
                gridTiles.Initialize(gridWidth, gridHeight);
            }

            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    if (gridTiles[x, y])
                    {
                        grid[x, y] = Instantiate(tilePrefab, grid.GetWorldPositionCentered(x, y),
                            Quaternion.identity, gridTransform);
                        grid[x, y].IsAvailable = true;
                    }
                    else
                    {
                        grid[x, y] = null;
                    }
                }
            }

            return grid;
        }
    }
}