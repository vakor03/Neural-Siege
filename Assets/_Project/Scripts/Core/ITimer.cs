using System;

namespace _Project.Scripts.Core
{
    public interface ITimer
    {
        float Duration { get; set; }
        void Start();
        void Stop();
        Action OnTimeElapsed { get; set; }
    }
}