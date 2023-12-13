using System.Collections.Generic;
using MEC;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public class SpawnerEnemy : Enemy
    {
        [SerializeField] private Enemy spawnEnemyPrefab;
        [SerializeField] private float spawnDelay;
        [SerializeField] private float spawnTime;
        [SerializeField] private float startSpawnDelay = 1;
        private float _spawnTimer;
        private bool _isSpawning = true;

        protected override void Awake()
        {
            base.Awake();
            _spawnTimer = spawnTime - startSpawnDelay;
        }

        protected override void Update()
        {
            base.Update();
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
            Timing.RunCoroutine(SpawnCoroutine());
        }

        private IEnumerator<float> SpawnCoroutine()
        {
            waypointsMover.SetSpeed(0);
            yield return Timing.WaitForSeconds(spawnDelay);
            Enemy instance = Instantiate(spawnEnemyPrefab, transform.position, Quaternion.identity, transform.parent);
            instance.Initialize(waypointsMover.WaypointsHolder);
            instance.WaypointsMover.SetWaypointIndex(waypointsMover.CurrentWaypointIndex);
         
            waypointsMover.SetSpeed(EnemyEffectsSystem.CurrentStats.speed);
        }
    }
}