using System;
using System.Collections.Generic;

namespace _Project.Scripts.Core.Enemies
{
    public class EnemiesAccessor
    {
        private readonly List<Enemy> _enemies = new();
        public event Action OnAllEnemiesDied;
        public event Action<Enemy> OnEnemyDied;
        public event Action<Enemy> OnEnemyDiedFromPlayer; 

        public void Add(Enemy enemy)
        {
            _enemies.Add(enemy);
            enemy.EnemyHealth.OnDiedFromPlayer += () => OnEnemyDiedFromPlayer?.Invoke(enemy);
        }

        public void Remove(Enemy enemy)
        {
            OnEnemyDied?.Invoke(enemy);
            _enemies.Remove(enemy);
            
            if (_enemies.Count == 0)
            {
                OnAllEnemiesDied?.Invoke();
            }
        }

        public List<Enemy> GetEnemies()
        {
            return _enemies;
        }

        public void Clear()
        {
            foreach (var enemy in _enemies.ToArray())
            {
                enemy.EnemyHealth.DestroySelf();
            }

            _enemies.Clear();
        }
    }
}