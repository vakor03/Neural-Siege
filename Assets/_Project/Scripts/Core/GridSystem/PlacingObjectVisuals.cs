using UnityEngine;

namespace _Project.Scripts.Core.GridSystem
{
    public class PlacingObjectVisuals : MonoBehaviour
    {
        [SerializeField] private Vector2Int size = Vector2Int.one;
        [SerializeField] private GameObject bottomLeftPoint;
        [SerializeField] private GameObject objectPrefab;
        
        public Vector2Int Size => size;
        public GameObject BottomLeftPoint => bottomLeftPoint;
        public GameObject ObjectPrefab => objectPrefab;
    }
}