using System.Linq;
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
        [SerializeField] private float sellPriceMultiplier = 0.6f;
        private PlacementSystem _placementSystem;
        private IShop _shop;

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        [Inject]
        private void Construct(IShop shop, PlacementSystem placementSystem)
        {
            _shop = shop;
            _placementSystem = placementSystem;
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
            var sellPrice = towerInfo.placementObject.price + upgradesPrice;

            _shop.EarnMoney((int)(sellPrice * sellPriceMultiplier));
            _placementSystem.RemoveObject(tower.gameObject.transform.position);
        }

        private TowerInfo GetTowerInfoOfType(TowerTypeSO typeSO)
        {
            return towerInfos.First(t => t.typeSO == typeSO);
        }
    }
}