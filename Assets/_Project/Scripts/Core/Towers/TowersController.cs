using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Core.GridSystem;
using _Project.Scripts.Core.Managers;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Core.Towers
{
    public class TowersController : IInitializable
    {
        private const string TOWER_INFO_DATABASE_PATH = "";
        private const float sellPriceMultiplier = 0.6f;

        private TowerInfoSO[] _towerInfos;
        private PlacementSystem _placementSystem;
        private IShop _shop;
        private InputManager _inputManager;
        public List<Tower> Towers { get; private set; } = new();

        [Inject]
        private void Construct(IShop shop, PlacementSystem placementSystem, InputManager inputManager)
        {
            _shop = shop;
            _placementSystem = placementSystem;
            _inputManager = inputManager;
        }

        public void Initialize()
        {
            _towerInfos = Resources.LoadAll<TowerInfoDatabaseSO>(TOWER_INFO_DATABASE_PATH).First().towerInfos;
        }

        private TowerTypeSO _activeTypeSO;

        public void StartPlacement(TowerTypeSO typeSO)
        {
            var towerInfo = GetTowerInfoOfType(typeSO);
            _placementSystem.StartPlacingObject(towerInfo.placementObject);
            _activeTypeSO = typeSO;

            _inputManager.OnClicked += OnClicked;
            _inputManager.OnExit += StopPlacement;
        }

        public void StopPlacement()
        {
            _activeTypeSO = null;
            _placementSystem.StopPlacingObject();

            _inputManager.OnClicked -= OnClicked;
            _inputManager.OnExit -= StopPlacement;
        }

        private void OnClicked()
        {
            if (!_inputManager.IsMousePositionValid())
            {
                return;
            }

            var mousePosition = _inputManager.GetCursorPosition();
            var towerInfo = GetTowerInfoOfType(_activeTypeSO);

            if (_placementSystem.ValidatePosition(mousePosition, towerInfo.placementObject))
            {
                DoPlacement(_activeTypeSO, mousePosition);
            }
        }

        public void DoPlacement(TowerTypeSO typeSO, Vector3 position)
        {
            var towerInfo = GetTowerInfoOfType(typeSO);
            var placementPrice = towerInfo.price;

            if (_shop.MoneyAmount < placementPrice) return;

            _shop.SpendMoney(placementPrice);
            var obj = _placementSystem.BuildObject(position, towerInfo.placementObject);
            RegisterTower(obj);
        }

        private void RegisterTower(GameObject obj)
        {
            Towers.Add(obj.GetComponentInChildren<Tower>());
        }

        public void UnregisterTower(GameObject obj)
        {
            Towers.Remove(obj.GetComponentInChildren<Tower>());
        }

        public void DoUpgrade(Tower tower)
        {
            var towerInfo = GetTowerInfoOfType(tower.TowerTypeSO);
            var upgradePrice = towerInfo.upgrade.price * (tower.UpgradeLevel + 1);

            if (_shop.MoneyAmount < upgradePrice) return;

            _shop.SpendMoney(upgradePrice);
            tower.ApplyUpgrade(towerInfo.upgrade);
        }

        public void DoSell(Tower tower)
        {
            var towerInfo = GetTowerInfoOfType(tower.TowerTypeSO);
            var upgradesPrice = towerInfo.upgrade.price * (tower.UpgradeLevel);
            var sellPrice = towerInfo.price + upgradesPrice;

            _shop.EarnMoney((int)(sellPrice * sellPriceMultiplier));
            UnregisterTower(tower.gameObject);
            _placementSystem.RemoveObject(tower.gameObject.transform.position);
        }

        private TowerInfoSO GetTowerInfoOfType(TowerTypeSO typeSO)
        {
            return _towerInfos.First(t => t.typeSO == typeSO);
        }
    }
}