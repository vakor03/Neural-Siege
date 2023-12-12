using UnityEngine;

namespace _Project.Scripts.Core.GridSystem
{
    public class PlacingObjectVisuals : MonoBehaviour
    {
        [SerializeField] private GameObject bottomLeftPoint;
        public GameObject BottomLeftPoint => bottomLeftPoint;
    }
}