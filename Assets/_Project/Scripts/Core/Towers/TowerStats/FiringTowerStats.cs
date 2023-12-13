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
    }
}