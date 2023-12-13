using UnityEngine;

namespace _Project.Scripts.Core.Towers.TowerStats
{
    public abstract class TowerStatsSO<T, TStats> : ScriptableObject where T : Tower where TStats : TowerStats<T>
    {
        public float range = 5;

        public abstract TStats GetTowerStats();
    }
}