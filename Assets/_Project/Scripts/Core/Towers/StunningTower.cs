using System.Collections.Generic;
using MEC;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public class StunningTower : Tower
    {
        [SerializeField] private StunningTowerStatsSO towerStatsSO;

        private bool _inProgress;
        private readonly List<Enemy> _enemiesInRange = new();

        private void Awake()
        {
            SetupCollider(towerStatsSO);
            Timing.RunCoroutine(AttackTimerCoroutine());
        }

        private IEnumerator<float> AttackTimerCoroutine()
        {
            _inProgress = true;
            do
            {
                yield return Timing.WaitForSeconds(1/towerStatsSO.fireRate);
                StunEnemiesInRange();
            } while (_enemiesInRange.Count > 0);

            _inProgress = false;
        }

        private void StunEnemiesInRange()
        {
            foreach (var enemy in _enemiesInRange)
            {
                enemy.ApplyEffect(new Effects.StunEffect(towerStatsSO.stunningDuration, enemy));
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
            if (_enemiesInRange.Count == 0)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.green;
            }

            Gizmos.DrawWireSphere(transform.position, towerStatsSO.range);
        }
    }
}