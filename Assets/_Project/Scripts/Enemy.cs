using System;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts
{
    public class Enemy : MonoBehaviour, IHaveHealth
    {
        [SerializeField] private float maxHealth = 5;
        [SerializeField, Self] private WaypointsMover waypointsMover;
        
        private PlayerBase _playerBase;
        private float _currentHealth;

        public float MaxHealth => maxHealth;

        public float CurrentHealth => _currentHealth;

        public event Action OnHealthChanged;
        public event Action OnDeath;

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        private void Awake()
        {
            _currentHealth = maxHealth;
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
    }
}