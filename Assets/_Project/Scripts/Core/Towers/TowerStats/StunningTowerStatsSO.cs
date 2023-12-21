using _Project.Scripts.Algorithms.GA.Structs;
using UnityEngine;

namespace _Project.Scripts.Core.Towers.TowerStats
{
    [CreateAssetMenu(menuName = "TowerStatsSO/StunningTower", fileName = "StunningTowerStatsSO", order = 0)]
    public class StunningTowerStatsSO : TowerStatsSO<StunningTower, StunningTowerStats>
    {
        public float stunningDuration = 1;
        public float fireRate = 0.25f;

        public override StunningTowerStats GetTowerStats()
        {
            return new StunningTowerStats(range, stunningDuration, fireRate);
        }
    }

    public class StunningTowerStats : TowerStats<StunningTower>
    {
        public float StunningDuration;
        public float FireRate;

        public StunningTowerStats(float range, float stunningDuration, float fireRate)
        {
            Range = range;
            StunningDuration = stunningDuration;
            FireRate = fireRate;
        }

        public override TowerStatsGA GetTowerStatsGA(TowerTypeSO towerType)
        {
            float slowingFactor = 1 / (1 + StunningDuration / (1 / FireRate - StunningDuration));
            return new TowerStatsGA
            {
                TowerType = towerType,
                DamagePerSecond = 0,
                SlowingFactor = slowingFactor,
                IsAoe = false,
                Range = Range
            };
        }
    }
}