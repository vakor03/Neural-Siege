using _Project.Scripts.Core.Effects;
using _Project.Scripts.Core.Enemies;
using _Project.Scripts.Core.Towers.TowerStats;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public class FreezingTower : Tower<FreezingTower, FreezingTowerStats>
    {
        private FreezeEffect _freezingEffect;

        protected override void Awake()
        {
            base.Awake();
            SetupCollider(TowerStatsController.CurrentStats);
            float freezingMultiplier = TowerStatsController.CurrentStats.FreezingMultiplier;
            _freezingEffect = new FreezeEffect(freezingMultiplier);
        }

        protected override void OnStatsChanged()
        {
            float freezingMultiplier = TowerStatsController.CurrentStats.FreezingMultiplier;
            float range = TowerStatsController.CurrentStats.Range;
            _freezingEffect.FreezeMultiplier = freezingMultiplier;
            TowerCollider2D.radius = range;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.EnemyStatsSystem.ApplyEffect(_freezingEffect);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.EnemyStatsSystem.RemoveEffect(_freezingEffect);
            }
        }

        private void OnDrawGizmos()
        {
            float range = TowerStatsController == null ? towerStatsSO.range : TowerStatsController.CurrentStats.Range;

            Gizmos.color = Color.green;

            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}