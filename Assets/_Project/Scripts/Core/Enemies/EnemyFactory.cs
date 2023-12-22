using AYellowpaper.SerializedCollections;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Core.Enemies
{
    public class EnemyFactory : MonoBehaviour, IEnemyFactory
    {
        [SerializeField] private SerializedDictionary<EnemyType, Enemy> enemyPrefabs;

        private DiContainer _container;
        private EnemiesAccessor _enemiesAccessor;
        
        [Inject]
        private void Construct(DiContainer container, EnemiesAccessor enemiesAccessor)
        {
            _container = container;
            _enemiesAccessor = enemiesAccessor;
        }

        public Enemy Create(EnemyType type, Vector3 position)
        {
            Enemy enemy = _container.InstantiatePrefabForComponent<Enemy>(enemyPrefabs[type], position, Quaternion.identity, null);
            _enemiesAccessor.Add(enemy);
            enemy.EnemyHealth.OnDeath += () => _enemiesAccessor.Remove(enemy);
            
            return enemy;
        }
    }
    
    public interface IEnemyFactory
    {
        Enemy Create(EnemyType type, Vector3 position);
    }
    
    public enum EnemyType
    {
        Casual,
        Fast,
        Spawned,
        Spawner,
        Tank
    }
}