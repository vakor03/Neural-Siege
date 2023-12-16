using System.Linq;
using _Project.Scripts.Core.GridSystem;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Core.Towers
{
    public class TowersController : MonoBehaviour
    {
        [SerializeField] private TowerInfo[] towerInfos;
        [SerializeField, Scene] private PlacementSystem placementSystem;
        [SerializeField, Scene] private Shop shop;
        [SerializeField] private float sellPriceMultiplier = 0.6f;

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        public void DoUpgrade(Tower tower)
        {
            var towerInfo = GetTowerInfoOfType(tower.TowerType);
            var upgradePrice = towerInfo.upgrade.price * (tower.UpgradeLevel + 1);

            if (shop.MoneyAmount < upgradePrice) return;

            shop.SpendMoney(upgradePrice);
            tower.ApplyUpgrade(towerInfo.upgrade);
        }

        public void DoSell(Tower tower)
        {
            var towerInfo = GetTowerInfoOfType(tower.TowerType);
            var upgradesPrice = towerInfo.upgrade.price * (tower.UpgradeLevel);
            var sellPrice = towerInfo.placementObject.price + upgradesPrice;

            shop.EarnMoney((int)(sellPrice * sellPriceMultiplier));
            placementSystem.RemoveObject(tower.gameObject.transform.position);
        }

        private TowerInfo GetTowerInfoOfType(TowerType type)
        {
            return towerInfos.First(t => t.type == type);
        }
    }
}