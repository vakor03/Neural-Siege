using System;
using UnityEngine;

namespace _Project.Scripts
{
    public class WaypointsMover : MonoBehaviour
    {
        [SerializeField] private WaypointsHolder waypointsHolder;
        [SerializeField] private float speed;

        public Action OnPathCompleted;
        private Transform[] _waypoints;
        private int _currentWaypointIndex;
        private bool _pathCompleted;

        private void Start()
        {
            _waypoints = waypointsHolder.GetWaypoints();
            _currentWaypointIndex = 0;
        }

        private void Update() // TODO: speed not working as expected
        {
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
    }
}