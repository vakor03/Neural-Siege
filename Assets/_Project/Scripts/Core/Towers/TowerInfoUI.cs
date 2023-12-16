using System;
using _Project.Scripts.Core.GridSystem;
using _Project.Scripts.Core.Towers.TowerUpgrades;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Core.Towers
{
    public class TowerInfoUI : MonoBehaviour
    {
        //TODO: Move nonUI logic to TowersController
        [SerializeField] private Tower tower;
        [SerializeField] private TowerUpgradeSO towerUpgradeSO;
        [SerializeField] private TowerType type;
        [SerializeField] private Button deleteButton;
        [SerializeField] private Button upgradeButton;
        [SerializeField, Scene] private PlacementSystem placementSystem;
        
        private void OnValidate()
        {
            this.ValidateRefs();
        }

        private float _timeSinceLastClick;

        private void Start()
        {
            deleteButton.onClick.AddListener(DeleteTower);
            upgradeButton.onClick.AddListener(UpgradeTower);
        }

        private void UpgradeTower()
        {
            TowersController.Instance.DoUpgrade(tower, towerUpgradeSO);
        }

        private void DeleteTower()
        {
            placementSystem.RemoveObject(tower.transform.position);
        }

        private void OnDestroy()
        {
            deleteButton.onClick.RemoveListener(DeleteTower);
            upgradeButton.onClick.RemoveListener(UpgradeTower);
        }

        private void Update()
        {
            _timeSinceLastClick += Time.deltaTime;
            if (_timeSinceLastClick > 0.1f)
            {
                HideIfClickedOutside(gameObject);
            }
        }

        private void OnDisable()
        {
            _timeSinceLastClick = 0;
        }

        private void HideIfClickedOutside(GameObject panel)
        {
            if (Input.GetMouseButton(0) && panel.activeSelf &&
                !RectTransformUtility.RectangleContainsScreenPoint(
                    panel.GetComponent<RectTransform>(),
                    Input.mousePosition,
                    Camera.main))
            {
                panel.SetActive(false);
            }
        }
    }
}