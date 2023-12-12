using _Project.Scripts.Core.Effects;
using _Project.Scripts.Extensions;
using JetBrains.Annotations;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public class FiringTower : MonoBehaviour
    {
        [SerializeField] private float range = 5;
        [SerializeField] private float fireRate = 1;
        [SerializeField] private float damagePerSecond = 1;
        [SerializeField] private float firingDuration = 0.5f;
        [SerializeField] private float coneAngle = 20;
        [SerializeField] private LayerMask enemyLayerMask;
        [SerializeField] private Transform shootPosition;


        [SerializeField, Self] private RotateTowards rotateTowards;
        [SerializeField, Self] private TargetChooseStrategy targetChooseStrategy;

        [CanBeNull] private Enemy _activeTarget;
        private ITimer _timer;

        private void OnValidate()
        {
            this.ValidateRefs();
        }

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

        private readonly Collider2D[] results = new Collider2D[100];

        private void FireAt(Vector3 coneCenter)
        {
            Vector3 coneDirection = coneCenter - transform.position;

            var enemiesInRange = Physics2D.OverlapCircleNonAlloc(transform.position, range, results, enemyLayerMask);
            for (var i = 0; i < enemiesInRange; i++)
            {
                var collider = results[i];
                Vector3 toTarget = collider.transform.position - transform.position;
                float angle = Vector3.Angle(coneDirection, toTarget);

                if (angle < coneAngle * 0.5f && collider.gameObject.TryGetComponent<Enemy>(out var enemy))
                {
                    enemy.ApplyEffect(new PoisonEffect(damagePerSecond, firingDuration, enemy));
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

            Gizmos.DrawWireSphere(transform.position, range);
            OnDrawGizmosSelected();
        }

        private void SetupCollider()
        {
            var collider2D = gameObject.GetOrAdd<CircleCollider2D>();
            collider2D.radius = range;
            collider2D.isTrigger = true;

            var rigidbody2D = gameObject.GetOrAdd<Rigidbody2D>();
            rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }

        void OnDrawGizmosSelected()
        {
            float halfFOV = coneAngle / 2.0f;
            var angle = Vector2.SignedAngle(transform.right, shootPosition.position - transform.position);

            Quaternion upRayRotation = Quaternion.AngleAxis(-halfFOV + angle, Vector3.forward);
            Quaternion downRayRotation = Quaternion.AngleAxis(halfFOV + angle, Vector3.forward);

            Vector3 upRayDirection = upRayRotation * transform.right * range;
            Vector3 downRayDirection = downRayRotation * transform.right * range;

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, upRayDirection);
            Gizmos.DrawRay(transform.position, downRayDirection);
        }
    }
}