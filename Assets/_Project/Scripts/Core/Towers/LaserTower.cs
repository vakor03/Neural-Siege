using System.Collections;
using _Project.Scripts.Core.Towers.TowerStats;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public class LaserTower : SingleTargetTower
    {
        [SerializeField] private LaserTowerStatsSO towerStatsSO;
        [SerializeField] private float laserDuration = 0.5f;
        private TowerStatsController<LaserTower, LaserTowerStats> _towerStatsController;

        private ITimer _timer;
        private EnemiesController _enemiesController;

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        private readonly RaycastHit2D[] _results = new RaycastHit2D[100];


        private void Awake()
        {
            InitTowerStats();
            float fireRate = _towerStatsController.CurrentStats.FireRate;
            float range = _towerStatsController.CurrentStats.Range;

            InitAttackTimer(fireRate);
            SetupCollider(_towerStatsController.CurrentStats);
            targetChooseStrategy.Range = range;
        }

        private void InitAttackTimer(float fireRate)
        {
            _timer = new TickTimer();
            _timer.Duration = 1 / fireRate;
            _timer.OnTimeElapsed += OnTimeElapsed;
        }

        private void InitTowerStats()
        {
            _towerStatsController = new(towerStatsSO);
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
            float damage = _towerStatsController.CurrentStats.Damage;
            float range = _towerStatsController.CurrentStats.Range;
            Vector2 direction = ActiveTarget!.transform.position - transform.position;
            int enemyLayer = _enemiesController.EnemyLayerMask;
            var size = Physics2D.RaycastNonAlloc(transform.position, direction, _results, range,
                enemyLayer);

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

        private void OnDrawGizmos()
        {
            float range = _towerStatsController.CurrentStats.Range;
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