using _Project.Scripts.Core.Towers.TowerStats;
using UnityEngine;

namespace _Project.Scripts.Core.Towers.TowerUpgrades
{
    [CreateAssetMenu(menuName = "TowerUpgrades/FiringTower", fileName = "FiringTowerUpgradeSO", order = 0)]
    public class FiringTowerUpgradeSO : TowerUpgradeSO<FiringTower, FiringTowerStats>
    {
        public float rangeAddition = 1;
        public float poisonMultiplier = 1.3f;
        public override void Apply(FiringTowerStats towerStats)
        {
            towerStats.Range += rangeAddition;
            towerStats.PoisonEffectStats.damagePerSecond *= poisonMultiplier;
        }   
    }
}