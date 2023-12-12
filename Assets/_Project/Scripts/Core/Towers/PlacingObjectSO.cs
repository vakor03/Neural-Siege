using _Project.Scripts.Core.GridSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Core.Towers
{
    [CreateAssetMenu(menuName = "Create PlacingObjectSO", fileName = "PlacingObjectSO", order = 0)]
    public class PlacingObjectSO : ScriptableObject
    {
        public PlacingObjectVisuals visuals;
        public GameObject tower;
        public Vector2Int size = Vector2Int.one;
    }
}