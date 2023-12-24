using System;
using System.Collections.Generic;
using _Project.Scripts.Core.Enemies;
using _Project.Scripts.Core.Towers.TowerStats;
using DG.Tweening;
using MEC;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public class StunningTower : Tower<StunningTower, StunningTowerStats>
    {
        private bool _inProgress;
        private readonly List<Enemy> _enemiesInRange = new();
        [SerializeField] private SpriteRenderer spriteToFade; // The sprite you want to fade
        [SerializeField] private GameObject stunVisualsGameObject;


        protected override void Awake()
        {
            base.Awake();
            SetupCollider(TowerStatsController.CurrentStats);
        }

        private void Start()
        {
            stunVisualsGameObject.transform.localScale = Vector3.zero;

        }

        protected override void OnStatsChanged()
        {
            float range = TowerStatsController.CurrentStats.Range;

            TowerCollider2D.radius = range;
        }

        private IEnumerator<float> AttackTimerCoroutine()
        {
            float fireRate = TowerStatsController.CurrentStats.FireRate;
            _inProgress = true;
            yield return Timing.WaitForSeconds(1 / fireRate);

            while (_enemiesInRange.Count > 0)
            {
                StunEnemiesInRange();
                yield return Timing.WaitForSeconds(1 / fireRate);
            }

            _inProgress = false;
        }

        private void StunEnemiesInRange()
        {
            stunVisualsGameObject.transform.localScale = Vector3.one * Range * 4f;
            spriteToFade.DOFade(0.6f, 0.01f).OnComplete(() => spriteToFade.DOFade(0, 0.5f));
            float stunningDuration = TowerStatsController.CurrentStats.StunningDuration;
            foreach (var enemy in _enemiesInRange)
            {
                enemy.EnemyStatsSystem.ApplyEffect(new Effects.StunEffect(stunningDuration, enemy.EnemyStatsSystem));
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<Enemy>(out var enemy))
            {
                _enemiesInRange.Add(enemy);
                if (!_inProgress)
                {
                    Timing.RunCoroutine(AttackTimerCoroutine());
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<Enemy>(out var enemy))
            {
                _enemiesInRange.Remove(enemy);
            }
        }

        private void OnDrawGizmos()
        {
            float range = TowerStatsController == null ? towerStatsSO.range : TowerStatsController.CurrentStats.Range;

            if (_enemiesInRange.Count == 0)
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