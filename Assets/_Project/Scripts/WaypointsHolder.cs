using System.Linq;
using _Project.Scripts.Extensions;
using UnityEngine;

namespace _Project.Scripts
{
    public class WaypointsHolder : MonoBehaviour
    {
        [SerializeField] private Transform[] waypoints;
        public Transform[] GetWaypoints() => waypoints;
        
        private void OnValidate()
        {
            waypoints = transform.Children().ToArray();
        }
    }
}