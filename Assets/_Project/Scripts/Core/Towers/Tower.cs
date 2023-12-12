using _Project.Scripts.Extensions;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public abstract class Tower : MonoBehaviour
    {
        protected void SetupCollider(TowerStatsSO towerStatsSO)
        {
            var collider2D = gameObject.GetOrAdd<CircleCollider2D>();
            collider2D.radius = towerStatsSO.range;
            collider2D.isTrigger = true;

            var rigidbody2D = gameObject.GetOrAdd<Rigidbody2D>();
            rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}