using _Project.Scripts.Core.Effects;
using _Project.Scripts.Core.Enemies;
using _Project.Scripts.Core.Towers.TowerStats;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public class FreezingTower : Tower<FreezingTower, FreezingTowerStats>
    {
        [SerializeField] private ParticleSystem visualEffect;

        private FreezeEffect _freezingEffect;

        protected override void Awake()
        {
            base.Awake();
            SetupCollider(TowerStatsController.CurrentStats);
            float freezingMultiplier = TowerStatsController.CurrentStats.FreezingMultiplier;
            _freezingEffect = new FreezeEffect(freezingMultiplier);
        }

        private void Start()
        {
            var speed = visualEffect.main.startSpeed;
            ParticleSystem.MainModule main = visualEffect.main;
            main.startLifetime = 2f * Range / speed.constant;
        }

        protected override void OnStatsChanged()
        {
            var speed = visualEffect.main.startSpeed;
            ParticleSystem.MainModule main = visualEffect.main;
            main.startLifetime = 2f * Range / speed.constant;

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