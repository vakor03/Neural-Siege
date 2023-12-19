using System;
using System.Collections.Generic;
using _Project.Scripts.Core.Enemies;
using MEC;

namespace _Project.Scripts.Core.Effects
{
    [Serializable]
    public struct PoisonEffectStats
    {
        public float damagePerSecond;
        public float duration;
    }

    public class PoisonEffect : Effect
    {
        private float _damagePerSecond;
        private float _duration;
        private readonly EnemyStatsSystem _enemy;
        private CoroutineHandle _timerCoroutine;

        public PoisonEffect(float damagePerSecond, float duration, EnemyStatsSystem enemy)
        {
            _damagePerSecond = damagePerSecond;
            _duration = duration;
            _enemy = enemy;

            _timerCoroutine = Timing.RunCoroutine(TimerCoroutine());
        }
        
        public PoisonEffect(PoisonEffectStats stats, EnemyStatsSystem enemy)
        {
            _damagePerSecond = stats.damagePerSecond;
            _duration = stats.duration;
            _enemy = enemy;

            _timerCoroutine = Timing.RunCoroutine(TimerCoroutine());
        }

        private IEnumerator<float> TimerCoroutine()
        {
            yield return Timing.WaitForSeconds(_duration);
            _enemy.RemoveEffect(this);
        }

        public override void Update(Enemy enemy, float deltaTime)
        {
            enemy.EnemyHealth.TakeDamage(_damagePerSecond * deltaTime);
        }

        public override void Reset()
        {
            Timing.KillCoroutines(_timerCoroutine);
            _timerCoroutine = Timing.RunCoroutine(TimerCoroutine());
        }
    }
}