using System;
using UnityEngine;

namespace _Project.Scripts
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private GameObject muzzlePrefab;
        [SerializeField] private GameObject hitPrefab;

        private Transform _parent;
        
        public void SetParent(Transform parent) => _parent = parent;
        public void SetSpeed(float speed) => this.speed = speed;

        private void Update()
        {
            transform.SetParent(null);
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        }
    }
}
