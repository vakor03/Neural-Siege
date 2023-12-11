using UnityEngine;

namespace _Project.Scripts.Core.GridSystem
{
    public class GridEditor : MonoBehaviour
    {
        private Grid<Tile> _grid;
        private Tile _tilePrefab;

        private void Update()
        {
            var mousePosition = Utils.GetMouseToWorldPosition();
            var gridPosition = _grid.GetGridPosition(mousePosition);
            
            if (Input.GetMouseButtonDown(0))
            {
                if (_grid[gridPosition.x, gridPosition.y] == null)
                {
                    _grid[gridPosition.x, gridPosition.y] = Instantiate(_tilePrefab,
                        _grid.GetWorldPositionCentered(gridPosition.x, gridPosition.y), Quaternion.identity,
                        transform);
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                _grid[gridPosition.x, gridPosition.y]?.Dispose();
                _grid[gridPosition.x, gridPosition.y] = null;
            }
        }

        public void Initialize(Grid<Tile> grid, Tile tilePrefab)
        {
            _grid = grid;
            _tilePrefab = tilePrefab;
        }
    }
}