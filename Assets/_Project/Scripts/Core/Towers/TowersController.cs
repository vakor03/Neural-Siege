using _Project.Scripts.Core.GridSystem;
using _Project.Scripts.Core.Towers.TowerUpgrades;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public class TowersController : MonoBehaviour
    {
        public static TowersController Instance { get; private set; }
        [SerializeField] private LayerMask towerLayerMask;
        [SerializeField] private PlacingObjectSO placingObjectSO;
        
        private void Awake()
        {
            Instance = this;
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 10,towerLayerMask);
                
                if (hit.collider != null)
                {
                    if (hit.collider.TryGetComponent(out TowerSelector towerSelector))
                    {
                        towerSelector.ToggleTowerUIInfo();
                    }
                }
            }
        }
        
        public void DoUpgrade(Tower tower, TowerUpgradeSO towerUpgradeSO)
        {
            tower.ApplyUpgrade(towerUpgradeSO);
        }
        
        public void DoSell(Tower tower, TowerType type)
        {
        }
    }
}