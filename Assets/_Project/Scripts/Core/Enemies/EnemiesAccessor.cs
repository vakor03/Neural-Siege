using System;
using System.Collections.Generic;

namespace _Project.Scripts.Core.Enemies
{
    public class EnemiesAccessor
    {
        private readonly List<Enemy> _enemies = new();
        public event Action OnAllEnemiesDied;

        public void Add(Enemy enemy)
        {
            _enemies.Add(enemy);
        }

        public void Remove(Enemy enemy)
        {
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