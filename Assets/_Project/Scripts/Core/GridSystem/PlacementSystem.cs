using _Project.Scripts.Core.Managers;
using _Project.Scripts.Extensions;
using KBCore.Refs;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Core.GridSystem
{
    public class PlacementSystem : MonoBehaviour
    {
        [SerializeField] private Grid grid;
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
        private IShop _shop;
        private InputManager _inputManager;


        [Inject]
        private void Construct(IShop shop, InputManager inputManager)
        {
            _shop = shop;
            _inputManager = inputManager;
        }

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

            var mousePosition = _inputManager.GetCursorPosition();
            var gridPosition = grid.WorldToCell(mousePosition);
            _currentPreview.transform.position = grid.CellToWorld(gridPosition);
        }

        public void StartPlacingObject(PlacementSystemObjectSO placementObject)
        {
            StopPlacingObject();
            _activeSO = placementObject;
            _currentPreview = Instantiate(_activeSO.previewPrefab);

            _isPlacingObject = true;
            _inputManager.OnClicked += OnClicked;
            _inputManager.OnExit += StopPlacingObject;
        }

        private void OnClicked()
        {
            if (!_inputManager.IsMousePositionValid())
            {
                return;
            }

            var mousePosition = _inputManager.GetCursorPosition();
            var gridPosition = grid.WorldToCell(mousePosition);

            if (_gridData.IsPlacementValid(_activeSO, gridPosition)
                && _shop.MoneyAmount >= _activeSO.price)
            {
                BuildObject(gridPosition, _activeSO);
                _shop.SpendMoney(_activeSO.price);
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

            _inputManager.OnClicked -= OnClicked;
            _inputManager.OnExit -= StopPlacingObject;
        }

        public void RemoveObject(Vector3 position)
        {
            var gridPosition = grid.WorldToCell(position);
            var placementObject = _gridData.GetObject(gridPosition);
            if (placementObject == null)
            {
                return;
            }

            _gridData.RemoveObject(gridPosition);
            Destroy(placementObject);
        }
    }
}