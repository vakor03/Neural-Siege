using _Project.Scripts.Core.Effects;
using _Project.Scripts.Extensions;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public class FreezingTower : MonoBehaviour
    {
        [SerializeField] private float range = 5;
        [SerializeField] private float freezingMultiplier = 0.5f;

        private FreezeEffect _freezingEffect;

        private void Awake()
        {
            SetupCollider();
            _freezingEffect = new FreezeEffect(freezingMultiplier);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.ApplyEffect(_freezingEffect);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.RemoveEffect(_freezingEffect);
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}