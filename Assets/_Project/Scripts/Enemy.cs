using System;
using UnityEngine;

namespace _Project.Scripts
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 5;
        
        private float _currentHealth;

        public float MaxHealth => maxHealth;

        public float CurrentHealth => _currentHealth;

        public event Action OnHealthChanged;
        
        private void Awake()
        {
            _currentHealth = maxHealth;
        }
        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            OnHealthChanged?.Invoke();
            
            if (_currentHealth <= 0)
            {
                DestroySelf();
            }
        }
        
        private void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}