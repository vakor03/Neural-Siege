using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Core.Configs;
using _Project.Scripts.Core.GridSystem;
using _Project.Scripts.Core.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Core.PathCreation
{
    public class ManualPathCreation : IPathCreationStrategy
    {
        private PlacementSystem _placementSystem;
        private InputManager _inputManager;
        private PathValidator _pathValidator = new();
        private PathFactory _pathFactory = new();

        private EnemyPathConfigSO _enemyPathConfig;
        
        private const int MinMagnitude = 5;

        private Vector2Int _start;
        private Vector2Int _end;
        private List<Vector2Int> _path = new();

        public event Action<Vector3[]> OnPathCreated;

        private ManualPathCreation(PlacementSystem placementSystem,
            InputManager inputSystem)
        {
            _placementSystem = placementSystem;
            _inputManager = inputSystem;
        }

        public void Initialize(EnemyPathConfigSO config)
        {
            _enemyPathConfig = config;
        }

        public void StartCreatingPath()
        {
            Reset();

            Vector2Int offset = (Vector2Int)_placementSystem.BottomLeft;
            (_start, _end) = GetStartAndFinish(_placementSystem.Size, offset);
            _placementSystem.BuildObject(_start, _enemyPathConfig.startTileSO);
            _placementSystem.BuildObject(_end, _enemyPathConfig.finishTileSO);

            _inputManager.OnClicked += OnClicked;
        }

        public void FinishCreatingPath()
        {
            _inputManager.OnClicked -= OnClicked;
            var isValid = _pathValidator.ValidatePath(_start, _end, _path);
            if (isValid)
            {
                var path = _pathFactory.Create(_start, _end, _path);
                OnPathCreated?.Invoke(path.Select(el => _placementSystem.Grid.GetCellCenterWorld((Vector3Int)el))
                    .ToArray());
            }
        }

        private void OnClicked()
        {
            var cursorPosition = _inputManager.GetCursorPosition();
            var gridPosition = (Vector2Int)_placementSystem.Grid.WorldToCell(cursorPosition);

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
                _placementSystem.BuildObject(gridPosition, _enemyPathConfig.enemyTileSO);
                if (_pathValidator.ValidatePath(_start, _end, _path))
                {
                    FinishCreatingPath();
                }
            }
        }

        private void Reset()
        {
            _inputManager.OnClicked -= OnClicked;
            _path.Clear();
            _start = Vector2Int.zero;
            _end = Vector2Int.zero;
        }

        private (Vector2Int start, Vector2Int end) GetStartAndFinish(Vector2Int gridSize, Vector2Int offset)
        {
            while (true)
            {
                Vector2Int start = new Vector2Int(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y)) + offset;
                Vector2Int end = new Vector2Int(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y)) + offset;

                if ((start - end).magnitude > MinMagnitude)
                {
                    return (start, end);
                }
            }
        }
    }
}