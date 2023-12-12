using _Project.Scripts.Core.Effects;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public class FreezingTower : Tower
    {
        [SerializeField] private FreezingTowerStatsSO towerStatsSO;

        private FreezeEffect _freezingEffect;

        private void Awake()
        {
            SetupCollider(towerStatsSO);
            _freezingEffect = new FreezeEffect(towerStatsSO.freezingMultiplier);
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            
            Gizmos.DrawWireSphere(transform.position, towerStatsSO.range);
        }
    }
}