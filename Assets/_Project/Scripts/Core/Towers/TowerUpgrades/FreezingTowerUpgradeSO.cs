using _Project.Scripts.Core.Towers.TowerStats;
using UnityEngine;

namespace _Project.Scripts.Core.Towers.TowerUpgrades
{
    [CreateAssetMenu(menuName = "TowerUpgrades/FreezingTower", fileName = "FreezingTowerUpgradeSO", order = 0)]
    public class FreezingTowerUpgradeSO : TowerUpgradeSO<FreezingTower, FreezingTowerStats>
    {
        public float rangeAddition = 1;
        public float freezingMultiplier = 1.3f;
        public override void Apply(FreezingTowerStats towerStats)
        {
            towerStats.Range += rangeAddition;
            towerStats.FreezingMultiplier *= freezingMultiplier;
        }   
    }
}