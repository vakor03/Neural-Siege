using System.Collections;
using _Project.Scripts.Extensions;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public class LaserTower : SingleTargetTower
    {
        [SerializeField] private float range = 5f;
        [SerializeField] private float damage = 10f;
        [SerializeField] private float fireRate = 1f;
        [SerializeField] private float laserDuration = 0.5f;

        private ITimer _timer;
        private EnemiesController _enemiesController;

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        private float nextFireTime;
        private readonly RaycastHit2D[] _results = new RaycastHit2D[100];


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
            _enemiesController = EnemiesController.Instance;
            _timer.Start();
        }

        private void OnTimeElapsed()
        {
            if (ActiveTarget == null) return;

            Shoot();
        }

        private void Shoot()
        {
            Vector2 direction = ActiveTarget!.transform.position - transform.position;
            int enemyLayer = _enemiesController.EnemyLayerMask;
            var size = Physics2D.RaycastNonAlloc(transform.position, direction, _results, range, enemyLayer);

            for (int i = 0; i < size; i++)
            {
                var hit = _results[i];

                Enemy enemy = hit.collider.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }

            StartCoroutine(LaserEffect());
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
            if (ActiveTarget == null)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position,
                    (ActiveTarget.transform.position - transform.position).normalized * range);
            }

            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}