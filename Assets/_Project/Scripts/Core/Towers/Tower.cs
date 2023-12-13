using _Project.Scripts.Core.Towers.TowerStats;
using _Project.Scripts.Extensions;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public abstract class Tower : MonoBehaviour
    {
        protected CircleCollider2D TowerCollider2D { get; private set; }
        protected void SetupCollider<T>(TowerStats<T> towerStats) where T : Tower
        {
            TowerCollider2D = gameObject.GetOrAdd<CircleCollider2D>();
            TowerCollider2D.radius = towerStats.Range;
            TowerCollider2D.isTrigger = true;

            var rigidbody2D = gameObject.GetOrAdd<Rigidbody2D>();
            rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}