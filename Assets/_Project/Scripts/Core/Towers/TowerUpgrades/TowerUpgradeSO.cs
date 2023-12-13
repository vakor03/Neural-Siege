using _Project.Scripts.Core.Towers.TowerStats;
using UnityEngine;

namespace _Project.Scripts.Core.Towers.TowerUpgrades
{
    public abstract class TowerUpgradeSO<T, TStats> : ScriptableObject where T : Tower where TStats : TowerStats<T>
    {
        public abstract void Apply(TStats towerStats);
    }
}   