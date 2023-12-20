using System.Collections.Generic;
using _Project.Scripts.Core.WaypointSystem;
using MEC;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _Project.Scripts.Core.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private float spawnRate;
        [FormerlySerializedAs("testWave")] [SerializeField] private EnemyWaveSO testWaveSO;
        
        private Vector3 _spawnPoint;
        private WaypointsHolder _waypointsHolder;
        private IEnemyFactory _enemyFactory;

        [Inject]
        private void Construct(IEnemyFactory enemyFactory)
        {
            _enemyFactory = enemyFactory;
        }

        public void Initialize(Vector3 spawnPoint, WaypointsHolder waypointsHolder)
        {
            _spawnPoint = spawnPoint;
            _waypointsHolder = waypointsHolder;
        }

        [ContextMenu("SpawnTestWave")]
        public void SpawnTestWave()
        {
            SpawnWave(testWaveSO);
        }

        public void SpawnWave(EnemyWaveSO enemyWaveSO)
        {
            Timing.RunCoroutine(SpawnCoroutine(enemyWaveSO, 1 / spawnRate));
        }

        private IEnumerator<float> SpawnCoroutine(EnemyWaveSO enemyWaveSO, float delay)
        {
            foreach (var enemyType in enemyWaveSO.enemies)
            {
                yield return Timing.WaitForSeconds(delay);
                var instance = _enemyFactory.Create(enemyType, _spawnPoint);
                instance.GetComponent<WaypointsMover>().Initialize(_waypointsHolder);
            }
        }
    }
}