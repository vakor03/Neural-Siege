using System;
using _Project.Scripts.Core.Effects;
using _Project.Scripts.Core.WaypointSystem;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts.Core.Enemies
{
    public class Enemy : MonoBehaviour, IHaveHealth
    {
        [SerializeField, Self] protected WaypointsMover waypointsMover;
        [SerializeField] private EnemyStats defaultStats;
        
        protected EnemyEffectsSystem EnemyEffectsSystem;
        private float _currentHealth;
        private PlayerBase _playerBase;

        public float MaxHealth => EnemyEffectsSystem.CurrentStats.maxHealth;
        private float _maxHealth;

        public float CurrentHealth => _currentHealth;
        public WaypointsMover WaypointsMover => waypointsMover;

        public event Action OnHealthChanged;
        public event Action OnDeath;

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        public void Initialize(WaypointsHolder waypointsHolder)
        {
            waypointsMover.Initialize(waypointsHolder);
        }

        protected virtual void Awake()
        {
            EnemyEffectsSystem = new EnemyEffectsSystem(defaultStats, this);
            EnemyEffectsSystem.OnStatsChanged += RecalculateStats;
            RecalculateStats();
            _currentHealth = _maxHealth;
        }

        protected virtual void Update()
        {
            EnemyEffectsSystem.Update();
        }

        private void RecalculateStats()
        {
            waypointsMover.SetSpeed(EnemyEffectsSystem.CurrentStats.speed);
            _maxHealth = EnemyEffectsSystem.CurrentStats.maxHealth;
        }

        protected virtual void Start()
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
            EnemyEffectsSystem.ApplyEffect(effect);
        }

        public void RemoveEffect(Effect effect)
        {
            EnemyEffectsSystem.RemoveEffect(effect);
        }

        private void OnDestroy()
        {
            EnemyEffectsSystem.OnStatsChanged -= RecalculateStats;
            EnemyEffectsSystem.Dispose();
        }
    }
}