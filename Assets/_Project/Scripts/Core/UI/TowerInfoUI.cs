using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Core.UI
{
    public class TowerInfoUI : MonoBehaviour
    {
        [SerializeField] private Button deleteButton;
        [SerializeField] private Button upgradeButton;

        public event Action OnUpgrade;
        public event Action OnDelete;

        private void Awake()
        {
            deleteButton.onClick.AddListener(DeleteTower);
            upgradeButton.onClick.AddListener(UpgradeTower);
        }

        private void UpgradeTower()
        {
            OnUpgrade?.Invoke();
        }

        private void DeleteTower()
        {
            OnDelete?.Invoke();
        }

        private void OnDestroy()
        {
            deleteButton.onClick.RemoveListener(DeleteTower);
            upgradeButton.onClick.RemoveListener(UpgradeTower);
        }
    }
}