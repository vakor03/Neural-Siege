using System;
using _Project.Scripts.Core.Enemies;
using UnityEngine;

namespace _Project.Scripts.Core.WaypointSystem
{
    public class WaypointsMover : MonoBehaviour
    {
        [SerializeField] private EnemyStatsSystem enemyStatsSystem;
        public WaypointsHolder WaypointsHolder { get; private set; }
        public int CurrentWaypointIndex => _currentWaypointIndex;

        public Action OnPathCompleted;
        private Transform[] _waypoints;
        private int _currentWaypointIndex;
        private bool _pathCompleted;

        public void Initialize(WaypointsHolder waypointsHolder)
        {
            WaypointsHolder = waypointsHolder;
            _waypoints = waypointsHolder.Waypoints;
            _currentWaypointIndex = 0;
        }

        private void Update()
        {
            var speed = enemyStatsSystem.CurrentStats.speed;
            Vector3 direction = (_waypoints[_currentWaypointIndex].position
                                 - transform.position).normalized;

            transform.Translate(direction * (speed * Time.deltaTime));

            var comparisonTolerance = 0.1f;
            if (Vector3.Distance(transform.position, _waypoints[_currentWaypointIndex].position) < comparisonTolerance)
            {
                _currentWaypointIndex++;
                if (_currentWaypointIndex >= _waypoints.Length)
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