using _Project.Scripts.Core.WaypointSystem;
using UnityEngine;

namespace _Project.Scripts.Core.GridSystem
{
    public class WaypointsHolderFactory
    {
        public WaypointsHolder Create(Vector3[] positions)
        {
            Transform[] waypoints = new Transform[positions.Length];
            for (var i = 0; i < positions.Length; i++)
            {
                GameObject waypoint = new GameObject($"Waypoint {i}");
                waypoint.transform.position = positions[i];
                waypoints[i] = waypoint.transform;
            }
            WaypointsHolder waypointsHolder = new()
            {
                Waypoints = waypoints
            };
            return waypointsHolder;
        }
    }
}