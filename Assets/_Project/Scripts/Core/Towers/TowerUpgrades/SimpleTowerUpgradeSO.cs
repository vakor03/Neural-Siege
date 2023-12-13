using _Project.Scripts.Core.Towers.TowerStats;
using UnityEngine;

namespace _Project.Scripts.Core.Towers.TowerUpgrades
{
    [CreateAssetMenu(menuName = "TowerUpgrades/SimpleTower", fileName = "SimpleTowerUpgradeSO", order = 0)]
    public class SimpleTowerUpgradeSO : TowerUpgradeSO<SimpleTower, SimpleTowerStats>
    {
        public float rangeAddition = 1;
        public float fireRateMultiplier = 1.3f;
        public override void Apply(SimpleTowerStats towerStats)
        {
            towerStats.Range += rangeAddition;
            towerStats.FireRate *= fireRateMultiplier;
        }   
    }
}