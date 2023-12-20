using _Project.Scripts.Core.WaypointSystem;
using UnityEngine;

namespace _Project.Scripts.Core.GridSystem
{
    public class WaypointsHolderFactory
    {
        public WaypointsHolder Create(Vector3[] positions)
        {
            WaypointsHolder waypointsHolder = new()
            {
                Waypoints = positions
            };
            return waypointsHolder;
        }
    }
}