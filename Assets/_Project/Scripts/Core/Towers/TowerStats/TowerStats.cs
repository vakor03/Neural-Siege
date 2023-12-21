using _Project.Scripts.Algorithms.GA.Structs;

namespace _Project.Scripts.Core.Towers.TowerStats
{
    public abstract class TowerStats<T> where T : Tower
    {
        public float Range;

        public abstract TowerStatsGA GetTowerStatsGA(TowerTypeSO towerType);
    }
}