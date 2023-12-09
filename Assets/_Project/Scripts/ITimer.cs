using System;

namespace _Project.Scripts
{
    public interface ITimer
    {
        float Duration { get; set; }
        void Start();
        void Stop();
        Action OnTimeElapsed { get; set; }
    }
}