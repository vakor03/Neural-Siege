using System.Collections.Generic;
using _Project.Scripts.Core.Effects;
using _Project.Scripts.Core.WaypointSystem;
using MEC;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Core.Enemies
{
    public class EnemySpawnBehaviour : MonoBehaviour
    {
        [SerializeField] private EnemyType spawnEnemyType;
        [SerializeField] private float spawnDelay = 0.5f;
        [SerializeField] private float spawnTime = 4;
        [SerializeField] private float startSpawnDelay = 1;

        [SerializeField] private EnemyStatsSystem enemyStatsSystem;
        [SerializeField] private WaypointsMover waypointsMover;

        private float _spawnTimer;
        private bool _isSpawning = true;
        private IEnemyFactory _enemyFactory;

        [Inject]
        private void Construct(IEnemyFactory enemyFactory)
        {
            _enemyFactory = enemyFactory;
        }

        protected void Awake()
        {
            _spawnTimer = spawnTime - startSpawnDelay;
        }

        protected void Update()
        {
            if (_isSpawning)
            {
                _spawnTimer += Time.deltaTime;
                if (_spawnTimer >= spawnTime)
                {
                    _spawnTimer = 0;
                    SpawnEnemy();
                }
            }
        }

        private void SpawnEnemy()
        {
            Timing.RunCoroutine(SpawnCoroutine().CancelWith(gameObject));
        }

        private IEnumerator<float> SpawnCoroutine()
        {
            enemyStatsSystem.ApplyEffect(new StunEffect(spawnDelay, enemyStatsSystem));
            yield return Timing.WaitForSeconds(spawnDelay);

            var instance = _enemyFactory.Create(spawnEnemyType, transform.position);
            var instanceWaypointsMover = instance.GetComponent<WaypointsMover>();
            instanceWaypointsMover.Initialize(waypointsMover.WaypointsHolder);
            instance.WaypointsMover.SetWaypointIndex(waypointsMover.CurrentWaypointIndex);
        }
    }
}