using System;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts.Core.Enemies
{
    public class EnemyHealth : ValidatedMonoBehaviour, IHaveHealth
    {
        [SerializeField, Self] private EnemyStatsSystem enemyStatsSystem;
        public float MaxHealth => enemyStatsSystem.CurrentStats.maxHealth;
        public float CurrentHealth { get; private set; }
        
        public event Action OnHealthChanged;
        public event Action OnDeath;

        private void Start()
        {
            CurrentHealth = MaxHealth;
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
            OnHealthChanged?.Invoke();

            if (CurrentHealth <= 0)
            {
                DestroySelf();
            }
        }
        
        public void DestroySelf()
        {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}