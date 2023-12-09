using System;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private GameObject muzzlePrefab;
        [SerializeField] private GameObject hitPrefab;
        [SerializeField,Self] private Collider2D projectileCollider;
        

        private Transform _target;
        private Transform _parent;

        public void SetParent(Transform parent) => _parent = parent;
        public void SetSpeed(float speed) => this.speed = speed;

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        private void Update()
        {
            transform.SetParent(null);
            var direction = (_target.position - transform.position).normalized;
            transform.position += direction * (speed * Time.deltaTime);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform == _target)
            {
                var enemy = other.GetComponent<Enemy>();
                enemy.TakeDamage(1);
                DestroyProjectile();
            }
        }

        private void DestroyProjectile()
        {
            Destroy(gameObject);
        }
    }
}