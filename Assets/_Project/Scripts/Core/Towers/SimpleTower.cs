using _Project.Scripts.Core.Towers.TowerStats;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public class SimpleTower : SingleTargetTower
    {
        [SerializeField] private SimpleTowerStatsSO towerStatsSO;
        [SerializeField] private Transform shootPosition;

        private ITimer _timer;
        private TowerStatsController<SimpleTower, SimpleTowerStats> _towerStatsController;

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        private void Awake()
        {
            InitTowerStats();
            float fireRate = _towerStatsController.CurrentStats.FireRate;
            float range = _towerStatsController.CurrentStats.Range;

            InitAttackTimer(fireRate);
            SetupCollider(_towerStatsController.CurrentStats);
            targetChooseStrategy.Range = range;
        }

        private void OnEnable()
        {
            _towerStatsController.OnStatsChanged += OnStatsChanged;
        }

        private void OnStatsChanged()
        {
            float fireRate = _towerStatsController.CurrentStats.FireRate;
            float range = _towerStatsController.CurrentStats.Range;

            _timer.Duration = 1 / fireRate;
            targetChooseStrategy.Range = range;
            TowerCollider2D.radius = range;
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
            _timer.Start();
        }

        private void OnDestroy()
        {
            _timer.OnTimeElapsed -= OnTimeElapsed;
            _timer.Stop();
            _towerStatsController.OnStatsChanged -= OnStatsChanged;
        }

        private void OnTimeElapsed()
        {
            if (ActiveTarget == null) return;

            ShootEnemy(ActiveTarget);
        }

        private void ShootEnemy(Enemy enemy)
        {
            var projectilePrefab = _towerStatsController.CurrentStats.ProjectilePrefab;
            Projectile projectile =
                Instantiate(projectilePrefab, shootPosition.position, Quaternion.identity);
            projectile.SetTarget(enemy.transform);
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
            }

            float range = _towerStatsController.CurrentStats.Range;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}