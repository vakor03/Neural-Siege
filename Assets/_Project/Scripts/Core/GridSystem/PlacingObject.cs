using UnityEngine;

namespace _Project.Scripts.Core.GridSystem
{
    public class PlacingObject : MonoBehaviour
    {
        [SerializeField] private Vector2Int size = Vector2Int.one;
        [SerializeField] private GameObject bottomLeftPoint;

        public Vector2Int Size => size;

        public GameObject BottomLeftPoint => bottomLeftPoint;
    }
}