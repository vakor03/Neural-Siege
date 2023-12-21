using _Project.Scripts.Algorithms.GA.Structs;
using UnityEngine;

namespace _Project.Scripts.Core.Towers.TowerStats
{
    [CreateAssetMenu(menuName = "TowerStatsSO/FreezingTower", fileName = "FreezingTowerStatsSO", order = 0)]
    public class FreezingTowerStatsSO : TowerStatsSO<FreezingTower, FreezingTowerStats>
    {
        public float freezingMultiplier = 0.5f;

        public override FreezingTowerStats GetTowerStats()
        {
            return new FreezingTowerStats(range, freezingMultiplier);
        }
    }

    public class FreezingTowerStats : TowerStats<FreezingTower>
    {
        public float FreezingMultiplier;

        public FreezingTowerStats(float range, float freezingMultiplier)
        {
            Range = range;
            FreezingMultiplier = freezingMultiplier;
        }

        public override TowerStatsGA GetTowerStatsGA(TowerTypeSO towerType)
        {
            return new TowerStatsGA
            {
                TowerType = towerType,
                DamagePerSecond = 0,
                SlowingFactor = FreezingMultiplier,
                IsAoe = false,
                Range = Range
            };
        }
    }
}