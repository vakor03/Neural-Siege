using AYellowpaper.SerializedCollections;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Core.Enemies
{
    public class EnemyFactory : MonoBehaviour, IEnemyFactory
    {
        [SerializeField] private SerializedDictionary<EnemyType, Enemy> enemyPrefabs;

        private DiContainer _container;
        
        [Inject]
        private void Construct(DiContainer container)
        {
            _container = container;
        }

        public Enemy Create(EnemyType type, Vector3 position)
        {
            Enemy enemy = _container.InstantiatePrefabForComponent<Enemy>(enemyPrefabs[type], position, Quaternion.identity, null);
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
        SmallCasual,
        Spawner,
        Tank
    }
}