using _Project.Scripts.Core.Enemies;
using _Project.Scripts.Core.Towers.TowerStats;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public class SimpleTower : SingleTargetTower<SimpleTower, SimpleTowerStats>
    {
        [SerializeField] private Transform shootPosition;

        private ITimer _timer;

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        protected override void Awake()
        {
            base.Awake();
            float fireRate = TowerStatsController.CurrentStats.FireRate;
            float range = TowerStatsController.CurrentStats.Range;

            InitAttackTimer(fireRate);
            SetupCollider(TowerStatsController.CurrentStats);
            targetChooseStrategy.Range = range;
        }

        protected override void OnStatsChanged()
        {
            float fireRate = TowerStatsController.CurrentStats.FireRate;
            float range = TowerStatsController.CurrentStats.Range;

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

        private void Start()
        {
            _timer.Start();
        }

        private void OnDestroy()
        {
            _timer.OnTimeElapsed -= OnTimeElapsed;
            _timer.Stop();
        }

        private void OnTimeElapsed()
        {
            if (ActiveTarget == null) return;

            ShootEnemy(ActiveTarget);
        }

        private void ShootEnemy(Enemy enemy)
        {
            var projectilePrefab = TowerStatsController.CurrentStats.ProjectilePrefab;
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

            float range = TowerStatsController == null ? towerStatsSO.range : TowerStatsController.CurrentStats.Range;

            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}