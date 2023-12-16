using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.Core.Managers
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private LayerMask gridLayer;
        private Camera _mainCamera;
        public event Action OnExit;
        public event Action OnClicked;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
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
    }
}