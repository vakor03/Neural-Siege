using System.Collections.Generic;
using _Project.Scripts.Core.WaypointSystem;
using MEC;
using UnityEngine;

namespace _Project.Scripts.Core.GridSystem
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private float spawnRate;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private WaypointsHolder waypointsHolder;
        
        [SerializeField] private EnemyWave testWave;

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
                Enemy instance = Instantiate(enemy, spawnPoint.position, Quaternion.identity, spawnPoint);
                instance.Initialize(waypointsHolder);
            }
        }
    }
}