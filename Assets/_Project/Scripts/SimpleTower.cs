using System.Collections.Generic;
using _Project.Scripts.Extensions;
using JetBrains.Annotations;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts
{
    public class SimpleTower : MonoBehaviour
    {
        [SerializeField] private float range;
        [SerializeField, Self] private RotateTowards rotateTowards;
        [SerializeField] private LayerMask enemyLayerMask;


        [CanBeNull] private Enemy _activeTarget;

        private readonly List<Collider2D> _colliders = new();

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        private void Awake()
        {
            SetupCollider();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TryChooseNewTarget();
            }
        }

        private void SetupCollider()
        {
            var collider2D = gameObject.GetOrAdd<CircleCollider2D>();
            collider2D.radius = range;
            collider2D.isTrigger = true;

            var rigidbody2D = gameObject.GetOrAdd<Rigidbody2D>();
            rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
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
                TryChooseNewTarget();
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

        private void TryChooseNewTarget() // TODO: return bool
        {
            var closestEnemy = GetClosestEnemy();
            if (closestEnemy == null) return;

            if (closestEnemy.TryGetComponent<Enemy>(out var enemy))
            {
                ChangeActiveTarget(enemy);
            }
        }

        [CanBeNull]
        private Collider2D GetClosestEnemy()
        {
            var contactFilter2D = new ContactFilter2D();
            contactFilter2D.layerMask = enemyLayerMask;
            var count = Physics2D.OverlapCircle(transform.position,
                range,
                contactFilter2D,
                _colliders);

            if (count == 0) return null;

            var closest = _colliders[0];
            var closestDistance = Vector2.Distance(transform.position, closest.transform.position);
            for (var i = 1; i < count; i++)
            {
                var distance = Vector2.Distance(transform.position, _colliders[i].transform.position);
                if (distance < closestDistance)
                {
                    closest = _colliders[i];
                    closestDistance = distance;
                }
            }

            return closest;
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
        }
    }
}