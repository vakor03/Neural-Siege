using _Project.Scripts.Core.Towers.TowerStats;
using UnityEngine;

namespace _Project.Scripts.Core.Towers.TowerUpgrades
{
    [CreateAssetMenu(menuName = "TowerUpgrades/StunningTower", fileName = "StunningTowerUpgradeSO", order = 0)]
    public class StunningTowerUpgradeSO : TowerUpgradeSO<StunningTower, StunningTowerStats>
    {
        public float stunMultiplier = 1.3f;
        public float rangeAddition = 1;
        public override void Apply(StunningTowerStats towerStats)
        {
            towerStats.StunningDuration *= stunMultiplier;
            towerStats.Range += rangeAddition;
        }   
    }
}