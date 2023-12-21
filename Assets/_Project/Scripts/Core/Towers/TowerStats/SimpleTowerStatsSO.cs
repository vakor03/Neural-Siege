using _Project.Scripts.Algorithms.GA.Structs;
using UnityEngine;

namespace _Project.Scripts.Core.Towers.TowerStats
{
    [CreateAssetMenu(menuName = "TowerStatsSO/SimpleTower", fileName = "SimpleTowerStatsSO", order = 0)]
    public class SimpleTowerStatsSO: TowerStatsSO<SimpleTower, SimpleTowerStats>
    {
        public float fireRate = 1;
        public Projectile projectilePrefab;
        public override SimpleTowerStats GetTowerStats()
        {
            return new SimpleTowerStats(range, fireRate, projectilePrefab);
        }
    }

    
    public class SimpleTowerStats : TowerStats<SimpleTower>
    {
        public float FireRate;
        public Projectile ProjectilePrefab;

        public SimpleTowerStats(float range, float fireRate, Projectile projectilePrefab)
        {
            FireRate = fireRate;
            ProjectilePrefab = projectilePrefab;
            Range = range;
        }

        public override TowerStatsGA GetTowerStatsGA(TowerTypeSO towerType)
        {
            return new TowerStatsGA
            {
                TowerType = towerType,
                DamagePerSecond = ProjectilePrefab.Damage * FireRate,
                SlowingFactor = 0,
                IsAoe = false,
                Range = Range
            };
        }
    }
}