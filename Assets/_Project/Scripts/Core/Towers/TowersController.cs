﻿using System.Linq;
using _Project.Scripts.Core.GridSystem;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _Project.Scripts.Core.Towers
{
    public class TowersController : MonoBehaviour
    {
        [SerializeField] private TowerInfo[] towerInfos;
        [SerializeField, Scene] private PlacementSystem placementSystem;
        [SerializeField] private float sellPriceMultiplier = 0.6f;
        private IShop _shop;

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        [Inject]
        private void Construct(IShop shop)
        {
            _shop = shop;
        }
         

        public void DoUpgrade(Tower tower)
        {
            var towerInfo = GetTowerInfoOfType(tower.TowerType);
            var upgradePrice = towerInfo.upgrade.price * (tower.UpgradeLevel + 1);

            if (_shop.MoneyAmount < upgradePrice) return;

            _shop.SpendMoney(upgradePrice);
            tower.ApplyUpgrade(towerInfo.upgrade);
        }

        public void DoSell(Tower tower)
        {
            var towerInfo = GetTowerInfoOfType(tower.TowerType);
            var upgradesPrice = towerInfo.upgrade.price * (tower.UpgradeLevel);
            var sellPrice = towerInfo.placementObject.price + upgradesPrice;

            _shop.EarnMoney((int)(sellPrice * sellPriceMultiplier));
            placementSystem.RemoveObject(tower.gameObject.transform.position);
        }

        private TowerInfo GetTowerInfoOfType(TowerType type)
        {
            return towerInfos.First(t => t.type == type);
        }
    }
}