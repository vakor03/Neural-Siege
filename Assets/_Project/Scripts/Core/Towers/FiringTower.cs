using _Project.Scripts.Core.Effects;
using _Project.Scripts.Core.Towers.TowerStats;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public class FiringTower : SingleTargetTower<FiringTower, FiringTowerStats>
    {
        [SerializeField] private Transform shootPosition;

        private ITimer _timer;
        private readonly Collider2D[] _results = new Collider2D[100];
        private EnemiesController _enemiesController;

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
            _enemiesController = EnemiesController.Instance;
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

            FireAt(ActiveTarget.transform.position);
        }

        private void FireAt(Vector3 coneCenter)
        {
            Vector3 coneDirection = coneCenter - transform.position;
            float range = TowerStatsController.CurrentStats.Range;
            float coneAngle = TowerStatsController.CurrentStats.ConeAngle;
            PoisonEffectStats poisonEffectStats = TowerStatsController.CurrentStats.PoisonEffectStats;
            
            int enemyLayerMask = _enemiesController.EnemyLayerMask;
            var enemiesInRange =
                Physics2D.OverlapCircleNonAlloc(transform.position, range, _results, enemyLayerMask);
            for (var i = 0; i < enemiesInRange; i++)
            {
                Vector3 toTarget = _results[i].transform.position - transform.position;
                float angle = Vector3.Angle(coneDirection, toTarget);

                if (angle < coneAngle * 0.5f &&
                    _results[i].gameObject.TryGetComponent<Enemy>(out var enemy))
                {
                    enemy.ApplyEffect(new PoisonEffect(poisonEffectStats, enemy));
                }
            }
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
            float coneAngle = TowerStatsController == null ? 20 : TowerStatsController.CurrentStats.ConeAngle;
            
            Gizmos.DrawWireSphere(transform.position, range);

            var angle = Vector2.SignedAngle(transform.right, shootPosition.position - transform.position);
            Utils.DrawGizmosCone(coneAngle,
                range,
                angle,
                transform,
                Color.blue);
        }
    }
}