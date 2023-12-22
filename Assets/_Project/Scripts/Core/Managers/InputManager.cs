using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace _Project.Scripts.Core.Managers
{
    public class InputManager : IInitializable, ITickable
    {
        private Camera _mainCamera;
        public event Action OnExit;
        public event Action OnClicked;

        public bool IsMouseOverUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        public Vector2 GetCursorPosition()
        {
            Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            return mousePosition;
        }

        public bool IsMousePositionValid()
        {
            return !IsMouseOverUI();
        }

        public void Initialize()
        {
            _mainCamera = Camera.main;
        }

        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                OnExit?.Invoke();
            }

            if (Input.GetMouseButtonDown(0))
            {
                OnClicked?.Invoke();
            }
        }
    }
}