using System;
using System.Collections.Generic;
using MEC;

namespace _Project.Scripts
{
    public class TickTimer : ITimer
    {
        public float Duration { get; set; }
        private CoroutineHandle _currentCoroutine;

        public void Start()
        {
            _currentCoroutine = Timing.RunCoroutine(TimerCoroutine());
        }

        private IEnumerator<float> TimerCoroutine()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(Duration);
                OnTimeElapsed?.Invoke();
            }
        }

        public void Stop()
        {
            Timing.KillCoroutines(_currentCoroutine);
        }

        public Action OnTimeElapsed { get; set; }
    }
}