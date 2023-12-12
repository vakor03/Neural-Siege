using System;
using System.Collections.Generic;
using _Project.Scripts.Core.Effects;
using _Project.Scripts.Core.GridSystem;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public class EnemyEffectsSystem : IDisposable
    {
        private EnemyStats _defaultStats;
        private List<Effect> _effects = new();
        private Enemy _enemy;

        private EnemyStats _currentStats;
        public EnemyStats CurrentStats => _currentStats;
        
        public event Action OnStatsChanged;

        public EnemyEffectsSystem(EnemyStats defaultStats, Enemy enemy)
        {
            _defaultStats = defaultStats;
            _enemy = enemy;
            
            RecalculateStats();
        }

        public void ApplyEffect(Effect effect)
        {
            if (_effects.Contains(effect))
            {
                effect.Reset();
            }
            else
            {
                _effects.Add(effect);
            }

            RecalculateStats();
            OnStatsChanged?.Invoke();
        }

        public void RemoveEffect(Effect effect)
        {
            _effects.Remove(effect);
            RecalculateStats();
            OnStatsChanged?.Invoke();
        }

        private void RecalculateStats()
        {
            _currentStats = _defaultStats;
            foreach (var effect in _effects)
            {
                _currentStats = effect.ApplyEffect(_currentStats);
            }
        }

        public void Update()
        {
            for (var i = 0; i < _effects.Count; i++)
            {
                var effect = _effects[i];
                effect.Update(_enemy, Time.deltaTime);
            }
        }

        public void Dispose()
        {
            _effects.Clear();
        }
    }
}