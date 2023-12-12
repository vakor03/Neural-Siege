using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    [CreateAssetMenu(menuName = "TowerStatsSO/LaserTower", fileName = "LaserTowerStatsSO", order = 0)]
    public class LaserTowerStatsSO : TowerStatsSO
    {
        public float damage = 1;
        public float fireRate = 1;
    }
}