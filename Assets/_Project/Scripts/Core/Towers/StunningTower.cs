using System.Collections.Generic;
using _Project.Scripts.Core.Towers.TowerStats;
using MEC;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public class StunningTower : Tower
    {
        [SerializeField] private StunningTowerStatsSO towerStatsSO;

        private bool _inProgress;
        private readonly List<Enemy> _enemiesInRange = new();
        private TowerStatsController<StunningTower, StunningTowerStats> _towerStatsController;

        private void Awake()
        {
            InitTowerStats();
            SetupCollider(_towerStatsController.CurrentStats);
            Timing.RunCoroutine(AttackTimerCoroutine());
        }

        private void InitTowerStats()
        {
            _towerStatsController = new(towerStatsSO);
        }

        private IEnumerator<float> AttackTimerCoroutine()
        {
            float fireRate = _towerStatsController.CurrentStats.FireRate;
            _inProgress = true;
            do
            {
                yield return Timing.WaitForSeconds(1 / fireRate);
                StunEnemiesInRange();
            } while (_enemiesInRange.Count > 0);

            _inProgress = false;
        }

        private void StunEnemiesInRange()
        {
            float stunningDuration = _towerStatsController.CurrentStats.StunningDuration;
            foreach (var enemy in _enemiesInRange)
            {
                enemy.ApplyEffect(new Effects.StunEffect(stunningDuration, enemy));
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<Enemy>(out var enemy))
            {
                _enemiesInRange.Add(enemy);
                if (!_inProgress)
                {
                    Timing.RunCoroutine(AttackTimerCoroutine());
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<Enemy>(out var enemy))
            {
                _enemiesInRange.Remove(enemy);
            }
        }

        private void OnDrawGizmos()
        {
            float range = _towerStatsController.CurrentStats.Range;
            if (_enemiesInRange.Count == 0)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.green;
            }

            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}