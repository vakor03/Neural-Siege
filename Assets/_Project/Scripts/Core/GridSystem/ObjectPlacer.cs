using _Project.Scripts.Extensions;
using UnityEngine;

namespace _Project.Scripts.Core.GridSystem
{
    public class ObjectPlacer : MonoBehaviour
    {
        [SerializeField] private PlacingObject placingObjectPrefab;

        private Grid<Tile> _grid;
        private bool _isPlacingObject;
        private PlacingObject _placingObject;
        private bool _isEnabled;

        public bool IsEnabled => _isEnabled;

        public void Initialize(Grid<Tile> grid)
        {
            _grid = grid;
        }
        
        public void Enable()
        {
            _isEnabled = true;
        }
        
        public void Disable()
        {
            if (_isPlacingObject)
            {
                Destroy(_placingObject.gameObject);
                _placingObject = null;
                _isPlacingObject = false;
            }
            _isEnabled = false;
        }

        private void Update()
        {
            if (!_isEnabled) return;
            //TODO: Refactor this
            if (Input.GetMouseButtonDown(0) && !_isPlacingObject)
            {
                _placingObject = Instantiate(placingObjectPrefab, Utils.GetMouseToWorldPosition(), Quaternion.identity);
                _isPlacingObject = true;
            }

            if (Input.GetMouseButtonDown(0) && _isPlacingObject)
            {
                bool isValid = ValidateGridPosition(_placingObject);

                if (isValid)
                {
                    PlaceObjectOnGrid(_placingObject);
                }
            }

            if (Input.GetMouseButtonDown(1) && _isPlacingObject)
            {
                Destroy(_placingObject.gameObject);
                _placingObject = null;
                _isPlacingObject = false;
            }
        }

        private void FixedUpdate()
        {
            if (!_isEnabled) return;
            if (_placingObject == null) return;

            // Snap to grid
            var mouseToWorldPosition = Utils.GetMouseToWorldPosition();
            var gridPosition = _grid.GetGridPosition(mouseToWorldPosition);

            _placingObject.transform.position = _grid.GetWorldPositionCentered(gridPosition);
        }

        private bool ValidateGridPosition(PlacingObject placingObject)
        {
            Vector3 bottomLeftTile = placingObject.BottomLeftPoint.transform.position.With(z: 0);
            int width = placingObject.Size.x;
            int height = placingObject.Size.y;

            var gridPosition = _grid.GetGridPosition(bottomLeftTile);

            if (CheckGridPositionOnGrid() || CheckFullObjectOnGrid())
            {
                return false;
            }

            return CheckAllTilesAreAvailable(height, width, gridPosition);

            bool CheckGridPositionOnGrid()
            {
                return gridPosition.x < 0 || gridPosition.x >= _grid.Width || gridPosition.y < 0 ||
                       gridPosition.y >= _grid.Height;
            }

            bool CheckFullObjectOnGrid()
            {
                return gridPosition.x + width > _grid.Width || gridPosition.y + height > _grid.Height;
            }
        }

        private bool CheckAllTilesAreAvailable(int height, int width, Vector2Int gridPosition)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (CheckTileIsAvailable(j, i))
                    {
                        return false;
                    }
                }
            }

            return true;


            bool CheckTileIsAvailable(int j, int i)
            {
                return _grid[gridPosition.x + j, gridPosition.y + i] == null
                       || !_grid[gridPosition.x + j, gridPosition.y + i].IsAvailable;
            }
        }

        private void PlaceObjectOnGrid(PlacingObject placingObject)
        {
            Vector3 bottomLeftTile = placingObject.BottomLeftPoint.transform.position.With(z: 0);
            int width = placingObject.Size.x;
            int height = placingObject.Size.y;

            var gridPosition = _grid.GetGridPosition(bottomLeftTile);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    _grid[gridPosition.x + j, gridPosition.y + i].IsAvailable = false;
                }
            }
        }
    }
}