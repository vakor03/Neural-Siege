using UnityEngine;

namespace _Project.Scripts.Infrastructure.States
{
    public class GameplayState : IState
    {
        public void Exit()
        {
        }

        public void Enter()
        {
            Debug.Log($"Entered {nameof(GameplayState)}");
        }
    }
}