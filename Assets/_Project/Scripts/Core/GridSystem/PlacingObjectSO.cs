using UnityEngine;

namespace _Project.Scripts.Core.GridSystem
{
    [CreateAssetMenu(menuName = "Create PlacingObjectSO", fileName = "PlacingObjectSO", order = 0)]
    public class PlacingObjectSO : ScriptableObject
    {
        public TowerType towerType;
        public PlacingObjectVisuals visuals;
        public GameObject tower;
        public Vector2Int size = Vector2Int.one;
        public int price = 10;
        public Sprite icon;
    }
    
    public enum TowerType
    {
        SimpleTower,
        FreezeTower,
        LaserTower,
        StunTower,
        PoisonTower,
    }
}