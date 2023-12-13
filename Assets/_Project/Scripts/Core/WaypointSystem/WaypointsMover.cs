using System;
using UnityEngine;

namespace _Project.Scripts.Core.WaypointSystem
{
    public class WaypointsMover : MonoBehaviour
    {
        private WaypointsHolder _waypointsHolder;
        
        private float _speed;
        public Action OnPathCompleted;
        private Transform[] _waypoints;
        private int _currentWaypointIndex;
        private bool _pathCompleted;
        
        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        public void Initialize(WaypointsHolder waypointsHolder)
        {
            this._waypointsHolder = waypointsHolder;
            _waypoints = waypointsHolder.GetWaypoints();
            _currentWaypointIndex = 0;
        }

        private void Update() // TODO: speed not working as expected
        {
            Vector3 direction = (_waypoints[_currentWaypointIndex].position
                                 - transform.position).normalized;

            transform.Translate(direction * (_speed * Time.deltaTime));

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