using _Project.Scripts.Core.Towers.TowerStats;
using UnityEngine;

namespace _Project.Scripts.Core.Towers.TowerUpgrades
{
    [CreateAssetMenu(menuName = "TowerUpgrades/LaserTower", fileName = "LaserTowerUpgradeSO", order = 0)]
    public class LaserTowerUpgradeSO : TowerUpgradeSO<LaserTower, LaserTowerStats>
    {
        public float rangeAddition = 1;
        public float fireRateMultiplier = 1.3f;
        public override void Apply(LaserTowerStats towerStats)
        {
            towerStats.Range += rangeAddition;
            towerStats.FireRate *= fireRateMultiplier;
        }   
    }
}