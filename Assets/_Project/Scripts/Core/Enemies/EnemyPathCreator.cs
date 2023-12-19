using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Core.GridSystem;
using KBCore.Refs;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Core.Enemies
{
    public class EnemyPathCreator : MonoBehaviour
    {
        [SerializeField] private PlacementSystemObjectSO enemyTileSO;
        [SerializeField] private PlacementSystemObjectSO startTileSO;
        [SerializeField] private PlacementSystemObjectSO finishTileSO;
        
        [SerializeField, Scene] private EnemySpawner enemySpawner;

        private PlacementSystem _placementSystem = new();
        private readonly BacktrackingPathGenerator _pathGenerator = new();
        private WaypointsHolderFactory _waypointsHolderFactory = new();

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        [Inject]
        private void Construct(PlacementSystem placementSystem)
        {
            _placementSystem = placementSystem;
        }

        [ContextMenu("Create Path")]
        public void CreatePath()
        {
            Vector2Int gridSize = _placementSystem.Size;
            (Vector2Int start, Vector2Int finish) = GetStartAndFinish(gridSize);
            var path = _pathGenerator.GeneratePath(gridSize, start, finish);
            var correctedPath = path.Select(p => (Vector3Int)p + _placementSystem.BottomLeft).ToList();
            BuildOnGrid(correctedPath);
            InitEnemySpawner(correctedPath);
        }

        public void InitEnemySpawner(List<Vector3Int> path)
        {
            var waypointsHolder = _waypointsHolderFactory.Create(
                path.Select(el => _placementSystem.Grid.GetCellCenterWorld(el)).ToArray());
 
            enemySpawner.Initialize(waypointsHolder.Waypoints[0], waypointsHolder);
        }

        private void BuildOnGrid(List<Vector3Int> path)
        {
            _placementSystem.BuildObject(path[0], startTileSO);
            for (var i = 1; i < path.Count - 1; i++)
            {
                _placementSystem.BuildObject(path[i], enemyTileSO);
            }

            _placementSystem.BuildObject(path[^1], finishTileSO);
        }

        private (Vector2Int start, Vector2Int finish) GetStartAndFinish(Vector2Int size)
        {
            return (new Vector2Int(0, 0), new Vector2Int(8, 8));
        }
    }
}