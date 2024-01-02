using System;
using System.Collections.Generic;
using _Project.Scripts.Core.Effects;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts.Core.Enemies
{
    public class EnemyStatsSystem : ValidatedMonoBehaviour
    {
        [SerializeField, Self] private Enemy enemy;

        private List<Effect> _effects = new();
        private EnemyStatsSO _defaultStatsSO;

        public EnemyStats CurrentStats { get; private set; }

        public event Action OnStatsChanged;

        public void Initialize(EnemyStatsSO defaultStats)
        {
            _defaultStatsSO = defaultStats;
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
            CurrentStats = _defaultStatsSO.enemyStats;
            foreach (var effect in _effects)
            {
                CurrentStats = effect.ApplyEffect(CurrentStats);
            }
        }

        public void Update()
        {
            for (var i = 0; i < _effects.Count; i++)
            {
                var effect = _effects[i];
                effect.Update(enemy, Time.deltaTime);
            }
        }
    }
}