using _Project.Scripts.Core.Effects;
using _Project.Scripts.Core.Towers.TowerStats;
using _Project.Scripts.Core.Towers.TowerUpgrades;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public class FiringTower : SingleTargetTower
    {
        [SerializeField] private FiringTowerStatsSO towerStatsSO;
        [SerializeField] private Transform shootPosition;
        [SerializeField] private FiringTowerUpgradeSO upgradeSO;
        
        private ITimer _timer;
        private readonly Collider2D[] _results = new Collider2D[100];
        private EnemiesController _enemiesController;
        private TowerStatsController<FiringTower, FiringTowerStats> _towerStatsController;

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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _towerStatsController.ApplyUpgrade(upgradeSO);
            }
        }

        private void OnStatsChanged()
        {
            float fireRate = _towerStatsController.CurrentStats.FireRate;
            float range = _towerStatsController.CurrentStats.Range;
            _timer.Duration = 1 / fireRate;
            targetChooseStrategy.Range = range;
            TowerCollider2D.radius = range;
        }

        private void OnDisable()
        {
            _towerStatsController.OnStatsChanged -= OnStatsChanged;
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
            float range = _towerStatsController.CurrentStats.Range;
            float coneAngle = _towerStatsController.CurrentStats.ConeAngle;
            PoisonEffectStats poisonEffectStats = _towerStatsController.CurrentStats.PoisonEffectStats;

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

            float range = _towerStatsController.CurrentStats.Range;
            float coneAngle = _towerStatsController.CurrentStats.ConeAngle;
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