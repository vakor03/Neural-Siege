using UnityEngine;

namespace _Project.Scripts
{
    public class PlayerBase : MonoBehaviour, IHaveHealth
    {
        public static PlayerBase Instance { get; private set; }
        [SerializeField] private float maxHealth = 5;
        
        private float _currentHealth;

        public float MaxHealth => maxHealth;
        public float CurrentHealth => _currentHealth;
        public event System.Action OnHealthChanged;
        public event System.Action OnDeath;
        
        private void Awake()
        {
            Instance = this;
            _currentHealth = maxHealth;
        }
        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            OnHealthChanged?.Invoke();
            
            if (_currentHealth <= 0)
            {
                OnDeath?.Invoke();
            }
        }
    }
}