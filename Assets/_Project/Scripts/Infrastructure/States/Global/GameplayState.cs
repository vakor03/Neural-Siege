using UnityEngine;

namespace _Project.Scripts.Infrastructure.States.Global
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