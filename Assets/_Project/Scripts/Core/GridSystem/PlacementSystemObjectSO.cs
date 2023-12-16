using UnityEngine;

namespace _Project.Scripts.Core.GridSystem
{
    [CreateAssetMenu(menuName = "Create PlacementSystemObjectSO", fileName = "_PlacementSO", order = 0)]
    public class PlacementSystemObjectSO : ScriptableObject
    {
        public new string name = "New Object";
        public int price = 10;
        public Vector2Int size = Vector2Int.one;
        public Sprite icon;
        public GameObject prefab;
        public GameObject previewPrefab;
    }
}   