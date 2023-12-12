using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    [CreateAssetMenu(menuName = "TowerStatsSO/FiringTower", fileName = "FiringTowerStatsSO", order = 0)]
    public class FiringTowerStatsSO : TowerStatsSO
    {
        public float fireRate = 1;
        public float coneAngle = 20;
    }
}