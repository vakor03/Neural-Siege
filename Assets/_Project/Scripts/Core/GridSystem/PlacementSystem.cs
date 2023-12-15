using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts.Core.GridSystem
{
    public class PlacementSystem : MonoBehaviour
    {
        [SerializeField] private Grid grid;
        [SerializeField] private InputManager inputManager;
        [SerializeField, Scene] private Shop shop;
        
        private PlacementSystemObjectSO _activeSO;
        private bool _isPlacingObject;
        private GameObject _currentPreview;
        private GridData _gridData = new();

        private void OnValidate()
        {
            this.ValidateRefs();
        }
        private void Start()
        {
            StopPlacingObject();
        }

        private void Update()
        {
            if (!_isPlacingObject)
            {
                return;
            }

            var mousePosition = inputManager.GetCursorPosition();
            var gridPosition = grid.WorldToCell(mousePosition);
            _currentPreview.transform.position = grid.CellToWorld(gridPosition);
        }

        public void StartPlacingObject(PlacementSystemObjectSO placementObject)
        {
            StopPlacingObject();
            _activeSO = placementObject;
            _currentPreview = Instantiate(_activeSO.previewPrefab);

            _isPlacingObject = true;
            inputManager.OnClicked += OnClicked;
            inputManager.OnExit += StopPlacingObject;
        }

        private void OnClicked()
        {
            if (!inputManager.IsMousePositionValid())
            {
                return;
            }
            var mousePosition = inputManager.GetCursorPosition();
            var gridPosition = grid.WorldToCell(mousePosition);
            
            if (_gridData.IsPlacementValid(_activeSO, gridPosition)
                && shop.MoneyAmount >= _activeSO.price)
            {
                BuildObject();
                shop.SpendMoney(_activeSO.price);
            }
        }

        private void BuildObject()
        {
            var mousePosition = inputManager.GetCursorPosition();
            var gridPosition = grid.WorldToCell(mousePosition);
            var bottomLeftPosition = grid.CellToWorld(gridPosition);

            var instance = Instantiate(_activeSO.prefab,
                bottomLeftPosition,
                Quaternion.identity);
            
            _gridData.AddObject(_activeSO, gridPosition, instance);
        }

        public void StopPlacingObject()
        {
            _isPlacingObject = false;
            Destroy(_currentPreview);
            
            inputManager.OnClicked -= OnClicked;
            inputManager.OnExit -= StopPlacingObject;
        }
    }
}