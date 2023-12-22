using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Algorithms;
using _Project.Scripts.Core.GridSystem;
using _Project.Scripts.Infrastructure.States;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Core.Enemies
{
    public class BacktrackingPathCreation : IPathCreationStrategy
    {
        private const int MinMagnitude = 5;
        private PlacementSystem _placementSystem;
        private BacktrackingPathGenerator _pathGenerator = new();
        private EnemyPathConfigSO _enemyPathConfig;

        public event Action<Vector3[]> OnPathCreated;

        private BacktrackingPathCreation(PlacementSystem placementSystem)
        {
            _placementSystem = placementSystem;
        }

        public void Initialize(EnemyPathConfigSO enemyPathConfigSO)
        {
            _enemyPathConfig = enemyPathConfigSO;
        }
        
        public void StartCreatingPath()
        {
            Vector2Int gridSize = _placementSystem.Size;
            (Vector2Int start, Vector2Int finish) = GetStartAndFinish(gridSize);
            var path = _pathGenerator.GeneratePath(gridSize, start, finish);
            var finalPath = path.Select(p => (Vector3Int)p + _placementSystem.BottomLeft).ToList();
            BuildOnGrid(finalPath);
            
            OnPathCreated?.Invoke(finalPath.Select(el => _placementSystem.Grid.GetCellCenterWorld(el)).ToArray());
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

        private (Vector2Int start, Vector2Int end) GetStartAndFinish(Vector2Int gridSize)
        {
            while (true)
            {
                Vector2Int start = new Vector2Int(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y));
                Vector2Int end = new Vector2Int(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y));

                if ((start - end).magnitude > MinMagnitude)
                {
                    return (start, end);
                }
            }
        }
    }
}