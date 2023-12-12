using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    [CreateAssetMenu(menuName = "TowerStatsSO/StunningTower", fileName = "StunningTowerStatsSO", order = 0)]
    public class StunningTowerStatsSO  : TowerStatsSO
    {
        public float stunningDuration = 1;
        public float fireRate = 0.25f;
    }
}