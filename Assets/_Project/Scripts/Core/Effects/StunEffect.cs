using System.Collections.Generic;
using _Project.Scripts.Core.Enemies;
using MEC;

namespace _Project.Scripts.Core.Effects
{
    public class StunEffect : Effect
    {
        private float _duration;
        private readonly Enemy _enemy;
        private CoroutineHandle _timerCoroutine;

        public StunEffect(float duration, Enemy enemy)
        {
            _duration = duration;
            _enemy = enemy;
            
            _timerCoroutine = Timing.RunCoroutine(TimerCoroutine());
        }

        private IEnumerator<float> TimerCoroutine()
        {
            yield return Timing.WaitForSeconds(_duration);
            _enemy.RemoveEffect(this);
        }

        public override EnemyStats ApplyEffect(EnemyStats enemy)
        {
            enemy.speed *= 0;
            return enemy;
        }

        public override void Reset()
        {
            Timing.KillCoroutines(_timerCoroutine);
            _timerCoroutine = Timing.RunCoroutine(TimerCoroutine());
        }
    }
}