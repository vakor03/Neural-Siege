using UnityEngine;

namespace _Project.Scripts
{
    public class TestGrid : MonoBehaviour
    {
        private Grid<Tile> _grid;
        [SerializeField] private Tile tilePrefab;
        [SerializeField] private int gridHeight = 10;
        [SerializeField] private int gridWidth = 10;
        [SerializeField] private float cellSize = 1f;
        [SerializeField] private Vector3 originPosition = Vector3.zero;
        [SerializeField] private GridSetup grid;

        private void Start()
        {
            _grid = new Grid<Tile>(gridWidth, gridHeight, cellSize, originPosition, true);
            InitializeGrid();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mousePosition = GetMouseToWorldPosition();
                var gridPosition = _grid.GetGridPosition(mousePosition);

                if (_grid[gridPosition.x, gridPosition.y] == null)
                {
                    _grid[gridPosition.x, gridPosition.y] = Instantiate(tilePrefab,
                        _grid.GetWorldPositionCentered(gridPosition.x, gridPosition.y), Quaternion.identity,
                        transform);
                }

                _grid[gridPosition.x, gridPosition.y].IsAvailable = !_grid[gridPosition.x, gridPosition.y].IsAvailable;
            }

            if (Input.GetMouseButtonDown(1))
            {
                var mousePosition = GetMouseToWorldPosition();
                var gridPosition = _grid.GetGridPosition(mousePosition);

                _grid[gridPosition.x, gridPosition.y]?.Dispose();
                _grid[gridPosition.x, gridPosition.y] = null;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveGrid();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                LoadGrid();
            }
        }

        public void SaveGrid()
        {
            grid.grid = new bool?[_grid.Height, _grid.Width];
            for (int i = 0; i < _grid.Height; i++)
            {
                for (int j = 0; j < _grid.Width; j++)
                {
                    if (_grid[j, i] == null)
                    {
                        grid.grid[i, j] = null;
                    }
                    else
                    {
                        grid.grid[i, j] = _grid[j, i].IsAvailable;
                    }
                }
            }
        }

        public void LoadGrid()
        {
            if (grid.grid == null) return;

            foreach (Transform tile in transform)
            {
                Destroy(tile.gameObject);
            }

            _grid = new Grid<Tile>(grid.grid.GetLength(1), grid.grid.GetLength(0), cellSize, originPosition, true);
            InitializeGrid();
            for (int i = 0; i < _grid.Height; i++)
            {
                for (int j = 0; j < _grid.Width; j++)
                {
                    if (grid.grid[i, j].HasValue)
                    {
                        _grid[j, i].IsAvailable = grid.grid[i, j].Value;
                    }
                    else
                    {
                        _grid[j, i].Dispose();
                        _grid[j, i] = null;
                    }
                }
            }
        }

        private void InitializeGrid()
        {
            for (int x = 0; x < _grid.Width; x++)
            {
                for (int y = 0; y < _grid.Height; y++)
                {
                    _grid[x, y] = Instantiate(tilePrefab, _grid.GetWorldPositionCentered(x, y), Quaternion.identity,
                        transform);
                }
            }
        }

        private static Vector3 GetMouseToWorldPosition()
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            return mousePosition;
        }
    }
}