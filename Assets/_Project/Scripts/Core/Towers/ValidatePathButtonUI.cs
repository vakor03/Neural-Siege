using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Core.Towers
{
    public class ValidatePathButtonUI : MonoBehaviour
    {
        [SerializeField] private Button button;
        
        public event Action OnClick;

        private void Awake()
        {
            button.onClick.AddListener(() => OnClick?.Invoke());
        }
        
        private void OnDestroy()
        {
            button.onClick.RemoveAllListeners();
        }
        
    }
}