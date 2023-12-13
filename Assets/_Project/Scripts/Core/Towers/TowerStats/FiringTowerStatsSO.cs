using _Project.Scripts.Core.Effects;
using UnityEngine;

namespace _Project.Scripts.Core.Towers.TowerStats
{
    [CreateAssetMenu(menuName = "TowerStatsSO/FiringTower", fileName = "FiringTowerStatsSO", order = 0)]
    public class FiringTowerStatsSO : TowerStatsSO<FiringTower, FiringTowerStats>
    {
        public float fireRate = 1;
        public float coneAngle = 20;
        public PoisonEffectStats poisonEffectStats;

        public override FiringTowerStats GetTowerStats()
        {
            return new FiringTowerStats(range, fireRate, coneAngle, poisonEffectStats);
        }
    }
}