using _Project.Scripts.Core.Effects;
using _Project.Scripts.Extensions;
using JetBrains.Annotations;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public class FiringTower : MonoBehaviour
    {
        [SerializeField] private FiringTowerStatsSO towerStatsSO;
        [SerializeField] private Transform shootPosition;
        [SerializeField] private PoisonEffectStats poisonEffectStats;

        [SerializeField, Self] private RotateTowards rotateTowards;
        [SerializeField, Self] private TargetChooseStrategy targetChooseStrategy;

        [CanBeNull] private Enemy _activeTarget;
        private ITimer _timer;
        private readonly Collider2D[] _results = new Collider2D[100];
        private EnemiesController _enemiesController;

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        private void Awake()
        {
            _timer = new TickTimer();
            _timer.Duration = 1 / towerStatsSO.fireRate;
            _timer.OnTimeElapsed += OnTimeElapsed;
            SetupCollider();
            targetChooseStrategy.Range = towerStatsSO.range;
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
            if (_activeTarget == null) return;

            FireAt(_activeTarget.transform.position);
        }

        private void FireAt(Vector3 coneCenter)
        {
            Vector3 coneDirection = coneCenter - transform.position;

            int enemyLayerMask = _enemiesController.EnemyLayerMask;
            var enemiesInRange =
                Physics2D.OverlapCircleNonAlloc(transform.position, towerStatsSO.range, _results, enemyLayerMask);
            for (var i = 0; i < enemiesInRange; i++)
            {
                Vector3 toTarget = _results[i].transform.position - transform.position;
                float angle = Vector3.Angle(coneDirection, toTarget);

                if (angle < towerStatsSO.coneAngle * 0.5f &&
                    _results[i].gameObject.TryGetComponent<Enemy>(out var enemy))
                {
                    enemy.ApplyEffect(new PoisonEffect(poisonEffectStats, enemy));
                }
            }
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

        private void OnDrawGizmos()
        {
            if (_activeTarget == null)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.green;
            }

            Gizmos.DrawWireSphere(transform.position, towerStatsSO.range);

            var angle = Vector2.SignedAngle(transform.right, shootPosition.position - transform.position);
            Utils.DrawGizmosCone(towerStatsSO.coneAngle,
                towerStatsSO.range,
                angle,
                transform,
                Color.blue);
        }

        private void SetupCollider()
        {
            var collider2D = gameObject.GetOrAdd<CircleCollider2D>();
            collider2D.radius = towerStatsSO.range;
            collider2D.isTrigger = true;

            var rigidbody2D = gameObject.GetOrAdd<Rigidbody2D>();
            rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}