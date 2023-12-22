using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Algorithms;
using _Project.Scripts.Core.GridSystem;
using _Project.Scripts.Infrastructure.States;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Core.Enemies
{
    public class EnemyPathCreator : IEnemyPathCreator
    {
        private PlacementSystem _placementSystem = new();
        private BacktrackingPathGenerator _pathGenerator = new();
        private WaypointsHolderFactory _waypointsHolderFactory = new();
        private EnemyPathConfigSO _enemyPathConfig;
        private EnemySpawner _enemySpawner;

        [Inject]
        private void Construct(PlacementSystem placementSystem,
            EnemySpawner enemySpawner)
        {
            _placementSystem = placementSystem;
            _enemySpawner = enemySpawner;
        }
        
        public event Action OnPathCreated;

        public void Initialize(EnemyPathConfigSO enemyPathConfigSO)
        {
            _enemyPathConfig = enemyPathConfigSO;
        }
        
        public void CreatePath()
        {
            Vector2Int gridSize = _placementSystem.Size;
            (Vector2Int start, Vector2Int finish) = GetStartAndFinish(gridSize);
            var path = _pathGenerator.GeneratePath(gridSize, start, finish);
            var correctedPath = path.Select(p => (Vector3Int)p + _placementSystem.BottomLeft).ToList();
            BuildOnGrid(correctedPath);
            InitEnemySpawner(correctedPath);
            
            OnPathCreated?.Invoke();
        }

        private void InitEnemySpawner(List<Vector3Int> path)
        {
            var waypointsHolder = _waypointsHolderFactory.Create(
                path.Select(el => _placementSystem.Grid.GetCellCenterWorld(el)).ToArray());

            _enemySpawner.Initialize(waypointsHolder.Waypoints[0], waypointsHolder);
        }

        private void BuildOnGrid(List<Vector3Int> path)
        {
            _placementSystem.BuildObject(path[0], _enemyPathConfig.startTileSO);
            for (var i = 1; i < path.Count - 1; i++)
            {
                _placementSystem.BuildObject(path[i], _enemyPathConfig.enemyTileSO);
            }

            _placementSystem.BuildObject(path[^1], _enemyPathConfig.finishTileSO);
        }

        private (Vector2Int start, Vector2Int finish) GetStartAndFinish(Vector2Int size)
        {
            return (new Vector2Int(0, 0), new Vector2Int(8, 8));
        }
    }
}