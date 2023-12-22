using System;
using System.Collections.Generic;
using MEC;

namespace _Project.Scripts.Core
{
    public class PlanningTimer
    {
        private float _planningTime = 5f; // TODO: Move to config
        private const float TimeBetweenTicks = 0.05f;

        private CoroutineHandle _timerCoroutine;
        public event Action OnTimerFinished;
        public event Action OnTimerTick;

        public float TimeLeft => _planningTime - CurrentTime;
        public float CurrentTime { get; private set; }

        public void Start()
        {
            Reset();
            _timerCoroutine = Timing.RunCoroutine(PlanningTimerCoroutine());
        }

        public void Reset()
        {
            Timing.KillCoroutines(_timerCoroutine);
            CurrentTime = 0f;
        }

        private IEnumerator<float> PlanningTimerCoroutine()
        {
            while (CurrentTime < _planningTime)
            {
                OnTimerTick?.Invoke();
                yield return Timing.WaitForSeconds(TimeBetweenTicks);
                CurrentTime += TimeBetweenTicks;
            }
            
            CurrentTime = _planningTime;
            OnTimerTick?.Invoke();
            OnTimerFinished?.Invoke();
        }
    }
}