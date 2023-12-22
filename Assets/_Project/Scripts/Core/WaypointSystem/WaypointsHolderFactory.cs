using UnityEngine;

namespace _Project.Scripts.Core.WaypointSystem
{
    public class WaypointsHolderFactory
    {
        public static WaypointsHolder Last; // TODO: Remove this
        public WaypointsHolder Create(Vector3[] positions)
        {
            WaypointsHolder waypointsHolder = new()
            {
                Waypoints = positions
            };
            
            Last = waypointsHolder;
            return waypointsHolder;
        }
    }
}