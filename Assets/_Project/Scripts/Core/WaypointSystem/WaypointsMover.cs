using System;
using _Project.Scripts.Core.Enemies;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts.Core.WaypointSystem
{
    public class WaypointsMover : ValidatedMonoBehaviour
    {
        [SerializeField, Self] private EnemyStatsSystem enemyStatsSystem;
        public WaypointsHolder WaypointsHolder { get; private set; }
        public int CurrentWaypointIndex => _currentWaypointIndex;

        public Action OnPathCompleted;
        private int _currentWaypointIndex;
        private bool _pathCompleted;

        public void Initialize(WaypointsHolder waypointsHolder)
        {
            WaypointsHolder = waypointsHolder;
            _currentWaypointIndex = 0;
        }

        private void Update()
        {
            var speed = enemyStatsSystem.CurrentStats.speed;
            var nextPosition = WaypointsHolder.Waypoints[_currentWaypointIndex];
            Vector3 direction = (nextPosition - transform.position).normalized;

            transform.Translate(direction * (speed * Time.deltaTime));

            var comparisonTolerance = 0.1f;
            if (Vector3.Distance(transform.position, nextPosition) < comparisonTolerance)
            {
                _currentWaypointIndex++;
                if (_currentWaypointIndex >= WaypointsHolder.Waypoints.Length)
                {
                    _currentWaypointIndex = 0;
                    _pathCompleted = true;
                    OnPathCompleted?.Invoke();
                }
            }
        }

        public void SetWaypointIndex(int waypointIndex)
        {
            _currentWaypointIndex = waypointIndex;
        }
    }
}