using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Core.Enemies;
using _Project.Scripts.Core.Managers;
using KBCore.Refs;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Core.GridSystem
{
    public class ManualPathCreation : MonoBehaviour
    {
        private PlacementSystem _placementSystem;
        [SerializeField] private PlacementSystemObjectSO enemyTileSO;
        [SerializeField] private PlacementSystemObjectSO startTileSO;
        [SerializeField] private PlacementSystemObjectSO finishTileSO;

        [SerializeField] private int minMagnitude = 5;
        [SerializeField, Scene] private EnemySpawner enemySpawner;

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        private InputManager _inputManager;
        private PathValidator _pathValidator = new();
        private PathFactory _pathFactory = new();
        private WaypointsHolderFactory _waypointsHolderFactory = new();

        [Inject]
        private void Construct(PlacementSystem placementSystem, InputManager inputSystem)
        {
            _placementSystem = placementSystem;
            _inputManager = inputSystem;
        }

        private Vector3Int _start;
        private Vector3Int _end;
        private List<Vector3Int> _path = new();

        [ContextMenu("Create Path")]
        public void StartCreatingPath()
        {
            _inputManager.OnClicked -= OnClicked;

            Vector3Int offset = _placementSystem.BottomLeft;
            (_start, _end) = GetStartAndFinish(_placementSystem.Size, offset);
            _placementSystem.BuildObject(_start, startTileSO);
            _placementSystem.BuildObject(_end, finishTileSO);

            _inputManager.OnClicked += OnClicked;
        }


        [ContextMenu("Finish Path")]    
        public void FinishCreatingPath()
        {
            _inputManager.OnClicked -= OnClicked;
            var isValid = _pathValidator.ValidatePath(_start, _end, _path);
            if (isValid)
            {
                var path = _pathFactory.Create(_start, _end, _path);
                InitEnemySpawner(path);
            }
        }

        public void InitEnemySpawner(List<Vector3Int> path)
        {
            var actualPathPositions = path.Select(_placementSystem.Grid.GetCellCenterWorld).ToArray();
            var waypointsHolder = _waypointsHolderFactory.Create(actualPathPositions);
 
            enemySpawner.Initialize(waypointsHolder.Waypoints[0], waypointsHolder);
        }

        private void OnClicked()
        {
            var cursorPosition = _inputManager.GetCursorPosition();
            var gridPosition = _placementSystem.Grid.WorldToCell(cursorPosition);

            if (_inputManager.IsMouseOverUI()) return;
            if (gridPosition == _start || gridPosition == _end) return;

            if (_placementSystem.IsPlaceTaken(gridPosition))
            {
                _path.Remove(gridPosition);
                _placementSystem.RemoveAt(gridPosition);
            }
            else
            {
                _path.Add(gridPosition);
                _placementSystem.BuildObject(gridPosition, enemyTileSO);
            }
        }

        private (Vector3Int start, Vector3Int end) GetStartAndFinish(Vector2Int gridSize, Vector3Int offset)
        {
            while (true)
            {
                Vector3Int start = new Vector3Int(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y), 0) + offset;
                Vector3Int end = new Vector3Int(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y), 0) + offset;

                if ((start - end).magnitude > minMagnitude)
                {
                    return (start, end);
                }
            }
        }
    }
}