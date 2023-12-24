using _Project.Scripts.Algorithms.GA.Structs;
using _Project.Scripts.Core.Effects;

namespace _Project.Scripts.Core.Towers.TowerStats
{
    public class FiringTowerStats : TowerStats<FiringTower>
    {
        public float FireRate;
        public float ConeAngle;
        public PoisonEffectStats PoisonEffectStats;

        public FiringTowerStats(float range, float fireRate, float coneAngle, PoisonEffectStats poisonEffectStats)
        {
            Range = range;
            FireRate = fireRate;
            ConeAngle = coneAngle;
            PoisonEffectStats = poisonEffectStats;
        }

        public override TowerStatsGA GetTowerStatsGA(TowerTypeSO towerType)
        {
            return new TowerStatsGA
            {
                TowerType = towerType,
                DamagePerSecond = PoisonEffectStats.damagePerSecond * FireRate,
                SlowingFactor = 0,
                IsAoe = true,
                Range = Range
            };
        }
    }
}