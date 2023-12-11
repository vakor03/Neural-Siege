using UnityEngine;

namespace _Project.Scripts.Core.GridSystem
{
    public class TestGrid : MonoBehaviour
    {
        [SerializeField] private Tile tilePrefab;
        [SerializeField] private GridSetupSO gridSetupSO;
        [SerializeField] private ObjectPlacer objectPlacer;

        private Grid<Tile> _grid;

        private void Start()
        {
            _grid = gridSetupSO.Load(tilePrefab, transform);
            objectPlacer.Initialize(_grid);
            objectPlacer.Enable();
        }

        private void Update()
        {
            // GridCreationBehaviour();

            if (Input.GetKeyDown(KeyCode.S))
            {
                gridSetupSO.Save(_grid);
            }
        }

        private void GridCreationBehaviour()
        {
            var mousePosition = Utils.GetMouseToWorldPosition();
            var gridPosition = _grid.GetGridPosition(mousePosition);
            
            if (Input.GetMouseButtonDown(0))
            {
                if (_grid[gridPosition.x, gridPosition.y] == null)
                {
                    _grid[gridPosition.x, gridPosition.y] = Instantiate(tilePrefab,
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
    }
}