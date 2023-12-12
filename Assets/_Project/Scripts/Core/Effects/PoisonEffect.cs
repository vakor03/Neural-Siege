using System.Collections.Generic;
using MEC;

namespace _Project.Scripts.Core.Effects
{
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