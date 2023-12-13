using System;
using _Project.Scripts.Core.Effects;
using _Project.Scripts.Core.WaypointSystem;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public class Enemy : MonoBehaviour, IHaveHealth
    {
        [SerializeField, Self] private WaypointsMover waypointsMover;
        [SerializeField] private EnemyStats defaultStats;

        private EnemyEffectsSystem _enemyEffectsSystem;
        private float _currentHealth;
        private PlayerBase _playerBase;

        public float MaxHealth => _enemyEffectsSystem.CurrentStats.maxHealth;
        private float _maxHealth;

        public float CurrentHealth => _currentHealth;

        public event Action OnHealthChanged;
        public event Action OnDeath;

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        private void Awake()
        {
            _enemyEffectsSystem = new EnemyEffectsSystem(defaultStats, this);
            _enemyEffectsSystem.OnStatsChanged += RecalculateStats;
            RecalculateStats();
            _currentHealth = _maxHealth;
        }

        private void RecalculateStats()
        {
            waypointsMover.SetSpeed(_enemyEffectsSystem.CurrentStats.speed);
            _maxHealth = _enemyEffectsSystem.CurrentStats.maxHealth;
        }

        private void Start()
        {
            _playerBase = PlayerBase.Instance;
            waypointsMover.OnPathCompleted += OnPathCompleted;
        }

        private void OnPathCompleted()
        {
            _playerBase.TakeDamage(1);
        }

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            OnHealthChanged?.Invoke();

            if (_currentHealth <= 0)
            {
                OnDeath?.Invoke();
                DestroySelf();
            }
        }

        private void DestroySelf()
        {
            Destroy(gameObject);
        }

        public void ApplyEffect(Effect effect)
        {
            _enemyEffectsSystem.ApplyEffect(effect);
        }

        public void RemoveEffect(Effect effect)
        {
            _enemyEffectsSystem.RemoveEffect(effect);
        }

        private void OnDestroy()
        {
            _enemyEffectsSystem.OnStatsChanged -= RecalculateStats;
            _enemyEffectsSystem.Dispose();
        }
    }
}