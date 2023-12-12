using System.Collections;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public class LaserTower : SingleTargetTower
    {
        [SerializeField] private LaserTowerStatsSO towerStatsSO;
        [SerializeField] private float laserDuration = 0.5f;

        private ITimer _timer;
        private EnemiesController _enemiesController;

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        private readonly RaycastHit2D[] _results = new RaycastHit2D[100];


        private void Awake()
        {
            _timer = new TickTimer();
            _timer.Duration = 1 / towerStatsSO.fireRate;
            _timer.OnTimeElapsed += OnTimeElapsed;
            SetupCollider(towerStatsSO);
            targetChooseStrategy.Range = towerStatsSO.range;
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
            var size = Physics2D.RaycastNonAlloc(transform.position, direction, _results, towerStatsSO.range,
                enemyLayer);

            for (int i = 0; i < size; i++)
            {
                var hit = _results[i];

                Enemy enemy = hit.collider.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.TakeDamage(towerStatsSO.damage);
                }
            }

            StartCoroutine(LaserEffect());
        }

        IEnumerator LaserEffect()
        {
            Debug.Log("Laser fired!");

            yield return new WaitForSeconds(laserDuration);
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
                    (ActiveTarget.transform.position - transform.position).normalized * towerStatsSO.range);
            }

            Gizmos.DrawWireSphere(transform.position, towerStatsSO.range);
        }
    }
}