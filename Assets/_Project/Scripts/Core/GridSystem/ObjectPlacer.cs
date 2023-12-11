using _Project.Scripts.Extensions;
using UnityEngine;

namespace _Project.Scripts.Core.GridSystem
{
    public class ObjectPlacer : MonoBehaviour
    {
        [SerializeField] private PlacingObjectVisuals placingObjectVisualsPrefab;

        private Grid<Tile> _grid;
        private bool _isPlacingObject;
        private PlacingObjectVisuals _currentVisuals;
        private Transform _placingTransform;

        public void Initialize(Grid<Tile> grid, Transform placingTransform)
        {
            _grid = grid;
            _placingTransform = placingTransform;
        }

        private void OnDisable()
        {
            if (_isPlacingObject)
            {
                Destroy(_currentVisuals.gameObject);
                _currentVisuals = null;
                _isPlacingObject = false;
            }
        }

        private void Update()
        {
            //TODO: Refactor this
            if (Input.GetMouseButtonDown(0) && _isPlacingObject)
            {
                bool isValid = ValidateGridPosition(_currentVisuals);

                if (isValid)
                {
                    PlaceObjectOnGrid(_currentVisuals);
                }
            }

            if (Input.GetMouseButtonDown(1) && _isPlacingObject)
            {
                Destroy(_currentVisuals.gameObject);
                _currentVisuals = null;
                _isPlacingObject = false;
            }

            if (Input.GetMouseButtonDown(0) && !_isPlacingObject)
            {
                _currentVisuals = Instantiate(placingObjectVisualsPrefab,
                    Utils.GetMouseToWorldPosition(), Quaternion.identity);
                _isPlacingObject = true;
            }
        }

        private void FixedUpdate()
        {
            if (_currentVisuals == null) return;

            // Snap to grid
            var mouseToWorldPosition = Utils.GetMouseToWorldPosition();
            var gridPosition = _grid.GetGridPosition(mouseToWorldPosition);

            _currentVisuals.transform.position = _grid.GetWorldPositionCentered(gridPosition);
        }

        private bool ValidateGridPosition(PlacingObjectVisuals placingObjectVisuals)
        {
            Vector3 bottomLeftTile = placingObjectVisuals.BottomLeftPoint.transform.position.With(z: 0);
            int width = placingObjectVisuals.Size.x;
            int height = placingObjectVisuals.Size.y;

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

        private void PlaceObjectOnGrid(PlacingObjectVisuals placingObjectVisuals)
        {
            Vector3 bottomLeftTile = placingObjectVisuals.BottomLeftPoint.transform.position.With(z: 0);
            int width = placingObjectVisuals.Size.x;
            int height = placingObjectVisuals.Size.y;

            var gridPosition = _grid.GetGridPosition(bottomLeftTile);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    _grid[gridPosition.x + j, gridPosition.y + i].IsAvailable = false;
                }
            }

            Instantiate(placingObjectVisuals.ObjectPrefab, placingObjectVisuals.transform.position, Quaternion.identity, _placingTransform);
        }
    }
}