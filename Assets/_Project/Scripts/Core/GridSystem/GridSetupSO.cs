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

            for (int i = 0; i < grid.Height; i++)
            {
                for (int j = 0; j < grid.Width; j++)
                {
                    gridTiles[i, j] = grid[j, i] != null;
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

            for (int i = 0; i < grid.Height; i++)
            {
                for (int j = 0; j < grid.Width; j++)
                {
                    if (gridTiles[i, j])
                    {
                        grid[j, i] = Instantiate(tilePrefab, grid.GetWorldPositionCentered(j, i),
                            Quaternion.identity, gridTransform);
                        grid[j, i].IsAvailable = true;
                    }
                    else
                    {
                        grid[j, i] = null;
                    }
                }
            }

            return grid;
        }
    }
}