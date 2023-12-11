using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public class TargetChooseStrategy : MonoBehaviour
    {
        private readonly List<Collider2D> _colliders = new();
        [SerializeField] private LayerMask enemyLayerMask;
        public float Range { get; set; }

        public bool TryChooseNewTarget([CanBeNull] out Enemy target)
        {
            target = null;
            var closestEnemy = GetClosestEnemy();
            if (closestEnemy == null) return false;

            if (closestEnemy.TryGetComponent<Enemy>(out var enemy))
            {
                target = enemy;
                return true;
            }

            return false;
        }

        [CanBeNull]
        private Collider2D GetClosestEnemy()
        {
            var contactFilter2D = new ContactFilter2D();
            contactFilter2D.layerMask = enemyLayerMask;
            var count = Physics2D.OverlapCircle(transform.position,
                Range,
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
    }
}