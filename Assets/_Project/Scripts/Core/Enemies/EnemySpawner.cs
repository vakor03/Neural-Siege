using System.Collections.Generic;
using _Project.Scripts.Core.WaypointSystem;
using MEC;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Core.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private float spawnRate;
        [SerializeField] private EnemyWave testWave;
        
        private Transform _spawnPoint;
        private WaypointsHolder _waypointsHolder;
        private IEnemyFactory _enemyFactory;

        [Inject]
        private void Construct(IEnemyFactory enemyFactory)
        {
            _enemyFactory = enemyFactory;
        }

        public void Initialize(Transform spawnPoint, WaypointsHolder waypointsHolder)
        {
            _spawnPoint = spawnPoint;
            _waypointsHolder = waypointsHolder;
        }

        [ContextMenu("SpawnTestWave")]
        public void SpawnTestWave()
        {
            SpawnWave(testWave);
        }

        public void SpawnWave(EnemyWave enemyWave)
        {
            Timing.RunCoroutine(SpawnCoroutine(enemyWave, 1 / spawnRate));
        }

        private IEnumerator<float> SpawnCoroutine(EnemyWave enemyWave, float delay)
        {
            foreach (var enemyType in enemyWave.enemies)
            {
                yield return Timing.WaitForSeconds(delay);
                var instance = _enemyFactory.Create(enemyType, _spawnPoint.position);
                instance.GetComponent<WaypointsMover>().Initialize(_waypointsHolder);
            }
        }
    }
}