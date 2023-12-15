using _Project.Scripts.Extensions;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts.Core.GridSystem
{
    public class PlacementSystem : MonoBehaviour
    {
        [SerializeField] private Grid grid;
        [SerializeField] private InputManager inputManager;
        [SerializeField, Scene] private Shop shop;
        [SerializeField] private Transform bottomLeftTransform;
        [SerializeField] private Transform topRightTransform;

        public Grid Grid => grid;
        public Vector3Int BottomLeft => grid.WorldToCell(bottomLeftTransform.position);
        public Vector3Int TopRight => grid.WorldToCell(topRightTransform.position);
        public Vector2Int Size => (TopRight - BottomLeft).ToVector2Int();

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
                BuildObject(gridPosition, _activeSO);
                shop.SpendMoney(_activeSO.price);
            }
        }

        public void BuildObject(Vector3Int gridPosition, PlacementSystemObjectSO placementObject)
        {
            var bottomLeftPosition = grid.CellToWorld(gridPosition);

            var instance = Instantiate(placementObject.prefab,
                bottomLeftPosition,
                Quaternion.identity,
                transform);

            _gridData.AddObject(placementObject, gridPosition, instance);
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