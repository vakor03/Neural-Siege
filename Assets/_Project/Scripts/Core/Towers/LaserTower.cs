using System.Collections;
using _Project.Scripts.Extensions;
using JetBrains.Annotations;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public class LaserTower : MonoBehaviour
    {
        [SerializeField] private float range = 5f;
        [SerializeField] private float damage = 10f;
        [SerializeField] private float fireRate = 1f;
        [SerializeField] private float laserDuration = 0.5f;
        [SerializeField] private LayerMask enemyLayer;

        [SerializeField, Self] private TargetChooseStrategy targetChooseStrategy;
        [SerializeField, Self] private RotateTowards rotateTowards;


        [CanBeNull] private Enemy _activeTarget;

        private ITimer _timer;

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        private float nextFireTime;

        private void Awake()
        {
            _timer = new TickTimer();
            _timer.Duration = 1 / fireRate;
            _timer.OnTimeElapsed += OnTimeElapsed;
            SetupCollider();
            targetChooseStrategy.Range = range;
        }

        private void Start()
        {
            _timer.Start();
        }

        private void OnTimeElapsed()
        {
            if (_activeTarget == null) return;

            Shoot();
        }

        private readonly RaycastHit2D[] results = new RaycastHit2D[100];

        private void Shoot()
        {
            Vector2 direction = _activeTarget!.transform.position - transform.position;
            var size = Physics2D.RaycastNonAlloc(transform.position, direction, results, range, enemyLayer);

            for (int i = 0; i < size; i++)
            {
                var hit = results[i];

                Enemy enemy = hit.collider.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }

            StartCoroutine(LaserEffect());
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_activeTarget == null &&
                other.gameObject.TryGetComponent<Enemy>(out var enemy))
            {
                ChangeActiveTarget(enemy);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_activeTarget != null &&
                other.gameObject == _activeTarget.gameObject)
            {
                RemoveActiveTarget();
                if (targetChooseStrategy.TryChooseNewTarget(out var enemy))
                {
                    ChangeActiveTarget(enemy);
                }
            }
        }

        private void ChangeActiveTarget(Enemy enemy)
        {
            _activeTarget = enemy;
            rotateTowards.Target = _activeTarget!.transform;
        }

        private void RemoveActiveTarget()
        {
            _activeTarget = null;
            rotateTowards.Target = null;
        }

        IEnumerator LaserEffect()
        {
            Debug.Log("Laser fired!");

            yield return new WaitForSeconds(laserDuration);
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
            if (_activeTarget == null)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position,
                    (_activeTarget.transform.position - transform.position).normalized * range);
            }

            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}