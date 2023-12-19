using System;
using UnityEngine;

namespace _Project.Scripts.Core.Enemies
{
    public class EnemyHealth : MonoBehaviour, IHaveHealth
    {
        [SerializeField] private EnemyStatsSystem enemyStatsSystem;
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