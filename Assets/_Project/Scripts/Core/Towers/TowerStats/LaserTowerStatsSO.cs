using UnityEngine;

namespace _Project.Scripts.Core.Towers.TowerStats
{
    [CreateAssetMenu(menuName = "TowerStatsSO/LaserTower", fileName = "LaserTowerStatsSO", order = 0)]
    public class LaserTowerStatsSO : TowerStatsSO<LaserTower, LaserTowerStats>
    {
        public float damage = 1;
        public float fireRate = 1;
        public override LaserTowerStats GetTowerStats()
        {
            return new LaserTowerStats(range,damage, fireRate);
        }
    }
    
    public class LaserTowerStats : TowerStats<LaserTower>
    {
        public float Damage;
        public float FireRate;
        
        public LaserTowerStats(float range, float damage, float fireRate)
        {
            Range = range;
            Damage = damage;
            FireRate = fireRate;
        }
    }
}