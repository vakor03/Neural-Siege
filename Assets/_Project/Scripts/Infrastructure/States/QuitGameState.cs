﻿namespace _Project.Scripts.Infrastructure.States
{
    public class QuitGameState : IState
    {
        public void Enter()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            UnityEngine.Application.Quit();
        }

        public void Exit()
        {
        }
    }
}