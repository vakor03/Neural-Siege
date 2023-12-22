using System.Linq;
using _Project.Scripts.Core.Managers;
using _Project.Scripts.Extensions;
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
        private InputManager _inputManager;


        [Inject]
        private void Construct(InputManager inputManager)
        {
            _inputManager = inputManager;
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
        }

        public GameObject BuildObject(Vector2Int gridPosition, PlacementSystemObjectSO placementObject)
        {
            var bottomLeftPosition = grid.CellToWorld((Vector3Int)gridPosition);

            var instance = Instantiate(placementObject.prefab,
                bottomLeftPosition,
                Quaternion.identity,
                transform);

            _gridData.AddObject(placementObject, (Vector3Int)gridPosition, instance);

            return instance;
        }

        public GameObject BuildObject(Vector3 position, PlacementSystemObjectSO placementObject)
        {
            var gridPosition = (Vector2Int)grid.WorldToCell(position);
            return BuildObject(gridPosition, placementObject);
        }

        public bool IsPlaceTaken(Vector3Int gridPosition)
        {
            return _gridData.IsCellTaken(gridPosition);
        }

        public bool IsPlaceTaken(Vector2Int gridPosition)
        {
            return IsPlaceTaken((Vector3Int)gridPosition);
        }

        public void StopPlacingObject()
        {
            _isPlacingObject = false;
            Destroy(_currentPreview);
        }

        public void RemoveObject(Vector3 position)
        {
            var gridPosition = grid.WorldToCell(position);
            var placementObject = _gridData.GetObject(gridPosition);
            if (placementObject == null)
            {
                return;
            }

            var placedObject = _gridData.RemoveObjectAt(gridPosition);
            Destroy(placedObject);
        }

        public void RemoveAt(Vector3Int gridPosition)
        {
            var placedObject = _gridData.RemoveObjectAt(gridPosition);
            Destroy(placedObject);
        }
        
        public void RemoveAt(Vector2Int gridPosition)
        {
            RemoveAt((Vector3Int)gridPosition);
        }

        public bool ValidatePosition(Vector3Int gridPosition, PlacementSystemObjectSO activeSO)
        {
            return _gridData.IsPlacementValid(activeSO, gridPosition);
        }

        public bool ValidatePosition(Vector3 position, PlacementSystemObjectSO activeSO)
        {
            var gridPosition = grid.WorldToCell(position);
            return ValidatePosition(gridPosition, activeSO);
        }

        public void Clear()
        {
            var placedObjects = _gridData._placedObjectsList.ToList();
            foreach (var placedObject in placedObjects)
            {
                Destroy(placedObject);
            }

            _gridData = new GridData();
        }
    }
}