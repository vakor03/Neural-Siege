using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Core.GridSystem;
using _Project.Scripts.Core.WaypointSystem;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts.Core.Enemies
{
    public class EnemyPathCreator : MonoBehaviour
    {
        [SerializeField] private PlacementSystemObjectSO enemyTileSO;
        [SerializeField] private PlacementSystemObjectSO startTileSO;
        [SerializeField] private PlacementSystemObjectSO finishTileSO;

        [SerializeField] private Transform waypointsParent;

        [SerializeField, Scene] private PlacementSystem placementSystem = new();
        [SerializeField, Scene] private EnemySpawner enemySpawner;

        private readonly BacktrackingPathGenerator _pathGenerator = new();

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        [ContextMenu("Create Path")]
        public void CreatePath()
        {
            Vector2Int gridSize = placementSystem.Size;
            (Vector2Int start, Vector2Int finish) = GetStartAndFinish(gridSize);
            var path = _pathGenerator.GeneratePath(gridSize, start, finish);
            var correctedPath = path.Select(p => (Vector3Int)p + placementSystem.BottomLeft).ToList();
            BuildOnGrid(correctedPath);
            InitEnemySpawner(correctedPath);
        }

        public void InitEnemySpawner(List<Vector3Int> path)
        {
            Transform[] waypoints = new Transform[path.Count];
            for (var i = 0; i < path.Count; i++)
            {
                GameObject waypoint = new GameObject($"Waypoint {i}");
                waypoint.transform.SetParent(waypointsParent);
                waypoint.transform.position = placementSystem.Grid
                    .GetCellCenterWorld(path[i]);
                waypoints[i] = waypoint.transform;
            }

            WaypointsHolder waypointsHolder = new()
            {
                Waypoints = waypoints
            };

            enemySpawner.Initialize(waypoints[0], waypointsHolder);
        }

        private void BuildOnGrid(List<Vector3Int> path)
        {
            placementSystem.BuildObject(path[0], startTileSO);
            for (var i = 1; i < path.Count - 1; i++)
            {
                placementSystem.BuildObject(path[i], enemyTileSO);
            }

            placementSystem.BuildObject(path[^1], finishTileSO);
        }

        private (Vector2Int start, Vector2Int finish) GetStartAndFinish(Vector2Int size)
        {
            return (new Vector2Int(0, 0), new Vector2Int(8, 8));
        }
    }
}