using System.Collections.Generic;
using _Project.Scripts.Extensions;
using MEC;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public class StunningTower : MonoBehaviour
    {
        [SerializeField] private float range = 5;
        [SerializeField] private float stunningDuration = 1;
        [SerializeField] private float timeBetweenStuns = 3;

        private bool _inProgress;
        private readonly List<Enemy> _enemiesInRange = new();

        private void Awake()
        {
            SetupCollider();
            Timing.RunCoroutine(AttackTimerCoroutine());
        }

        private IEnumerator<float> AttackTimerCoroutine()
        {
            _inProgress = true;
            do
            {
                yield return Timing.WaitForSeconds(timeBetweenStuns);
                StunEnemiesInRange();
            } while (_enemiesInRange.Count > 0);

            _inProgress = false;
        }

        private void StunEnemiesInRange()
        {
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

        private void SetupCollider()
        {
            var collider2D = gameObject.GetOrAdd<CircleCollider2D>();
            collider2D.radius = range;
            collider2D.isTrigger = true;

            var rigidbody2D = gameObject.GetOrAdd<Rigidbody2D>();
            rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
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

            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}