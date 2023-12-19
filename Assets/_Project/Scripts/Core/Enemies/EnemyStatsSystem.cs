using System;
using System.Collections.Generic;
using _Project.Scripts.Core.Effects;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts.Core.Enemies
{
    public class EnemyStatsSystem : MonoBehaviour
    {
        [SerializeField] private EnemyStats defaultStats;
        [SerializeField] private Enemy enemy;
        
        private List<Effect> _effects = new();

        public EnemyStats CurrentStats { get; private set; }

        public event Action OnStatsChanged;

        private void Awake()
        {
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
            CurrentStats = defaultStats;
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