using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    [CreateAssetMenu(menuName = "TowerStatsSO/SimpleTower", fileName = "SimpleTowerStatsSO", order = 0)]
    public class SimpleTowerStatsSO : TowerStatsSO
    {
        public float attackInterval = 1;
        public Projectile projectilePrefab;
    }
}