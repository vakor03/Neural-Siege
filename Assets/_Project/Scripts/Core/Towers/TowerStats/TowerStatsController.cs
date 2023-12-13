using System;
using System.Collections.Generic;
using _Project.Scripts.Core.Towers.TowerUpgrades;

namespace _Project.Scripts.Core.Towers.TowerStats
{
    public class TowerStatsController<T, TStats> where T : Tower where TStats : TowerStats<T>
    {
        private TowerStatsSO<T, TStats> _defaultStatsSO;
        private List<TowerUpgradeSO<T, TStats>> _upgrades;
        
        public Action OnStatsChanged;

        public TStats CurrentStats { get; private set; }

        public TowerStatsController(TowerStatsSO<T, TStats> defaultStatsSO)
        {
            _defaultStatsSO = defaultStatsSO;
            _upgrades = new List<TowerUpgradeSO<T, TStats>>();
            RecalculateStats();
        }

        public void ApplyUpgrade(TowerUpgradeSO<T, TStats> upgrade)
        {
            _upgrades.Add(upgrade);
            RecalculateStats();
        }

        public void RemoveUpgrade(TowerUpgradeSO<T, TStats> upgrade)
        {
            _upgrades.Remove(upgrade);
            RecalculateStats();
        }

        private void RecalculateStats()
        {
            CurrentStats = _defaultStatsSO.GetTowerStats();
            foreach (var upgrade in _upgrades)
            {
                upgrade.Apply(CurrentStats);
            }
            OnStatsChanged?.Invoke();
        }
    }
}