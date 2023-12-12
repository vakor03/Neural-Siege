using System.Collections.Generic;
using MEC;

namespace _Project.Scripts.Core.GridSystem
{
    public abstract class Effect
    {
        public virtual EnemyStats ApplyEffect(EnemyStats enemy)
        {
            return enemy;
        }

        public virtual void Reset()
        {
        }

        public virtual void Update(Enemy enemy, float deltaTime)
        {
        }
    }

    public class FreezeEffect : Effect
    {
        private readonly float _speedMultiplier;

        public FreezeEffect(float freezingMultiplier)
        {
            _speedMultiplier = freezingMultiplier;
        }

        public override EnemyStats ApplyEffect(EnemyStats enemy)
        {
            enemy.speed *= _speedMultiplier;
            return enemy;
        }

        public override void Reset()
        {
            // nothing to reset
        }
    }

    public class PoisonEffect : Effect
    {
        private float _damagePerSecond;
        private float _duration;
        private readonly Enemy _enemy;
        private CoroutineHandle _timerCoroutine;

        public PoisonEffect(float damagePerSecond, float duration, Enemy enemy)
        {
            _damagePerSecond = damagePerSecond;
            _duration = duration;
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
            enemy.TakeDamage(_damagePerSecond * deltaTime);
        }

        public override void Reset()
        {
            Timing.KillCoroutines(_timerCoroutine);
            _timerCoroutine = Timing.RunCoroutine(TimerCoroutine());
        }
    }
}