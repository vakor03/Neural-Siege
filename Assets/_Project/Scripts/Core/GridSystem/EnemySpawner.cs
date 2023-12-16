using System.Collections.Generic;
using _Project.Scripts.Core.WaypointSystem;
using MEC;
using UnityEngine;

namespace _Project.Scripts.Core.GridSystem
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private float spawnRate;
        
        [SerializeField] private EnemyWave testWave;
        private Transform _spawnPoint;
        private WaypointsHolder _waypointsHolder;

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
            foreach (Enemy enemy in enemyWave.enemies)
            {
                yield return Timing.WaitForSeconds(delay);
                Enemy instance = Instantiate(enemy, _spawnPoint.position, Quaternion.identity, _spawnPoint);
                instance.Initialize(_waypointsHolder);
            }
        }
    }
}