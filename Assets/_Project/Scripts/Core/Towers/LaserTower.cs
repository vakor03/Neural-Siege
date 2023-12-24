using System;
using _Project.Scripts.Core.Enemies;
using _Project.Scripts.Core.Towers.TowerStats;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public class LaserTower : SingleTargetTower<LaserTower, LaserTowerStats>
    {
        private ITimer _timer;
        private EnemiesController _enemiesController;
        private readonly RaycastHit2D[] _results = new RaycastHit2D[100];
        
        public event Action OnAttacked; 

        protected override void Awake()
        {
            base.Awake();
            float fireRate = TowerStatsController.CurrentStats.FireRate;
            float range = TowerStatsController.CurrentStats.Range;

            InitAttackTimer(fireRate);
            SetupCollider(TowerStatsController.CurrentStats);
            targetChooseStrategy.Range = range;
        }

        private void InitAttackTimer(float fireRate)
        {
            _timer = new TickTimer();
            _timer.Duration = 1 / fireRate;
            _timer.OnTimeElapsed += OnTimeElapsed;
        }

        protected override void OnStatsChanged()
        {
            float fireRate = TowerStatsController.CurrentStats.FireRate;
            float range = TowerStatsController.CurrentStats.Range;

            _timer.Duration = 1 / fireRate;
            targetChooseStrategy.Range = range;
            TowerCollider2D.radius = range;
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
            float damage = TowerStatsController.CurrentStats.Damage;
            float range = TowerStatsController.CurrentStats.Range;
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
                    enemy.EnemyHealth.TakeDamage(damage);
                }
            }

            OnAttacked?.Invoke();
        }

        private void OnDrawGizmos()
        {
            float range = TowerStatsController == null ? towerStatsSO.range : TowerStatsController.CurrentStats.Range;

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