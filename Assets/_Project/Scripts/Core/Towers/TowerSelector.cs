using _Project.Scripts.Core.Managers;
using _Project.Scripts.Core.UI;
using KBCore.Refs;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Core.Towers
{
    public class TowerSelector : MonoBehaviour
    {
        [SerializeField] private LayerMask towerLayerMask;
        [SerializeField] private TowerInfoUI towerInfoUI;
        [SerializeField] private Vector2 towerInfoOffset;

        private Camera _camera;
        private Tower _selectedTower;
        private InputManager _inputManager;
        private TowersController _towersController;

        [Inject]
        private void Construct(InputManager inputManager, TowersController towersController)
        {
            _inputManager = inputManager;
            _towersController = towersController;
        }

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Start()
        {
            _inputManager.OnClicked += OnClicked;

            towerInfoUI.OnUpgrade += UpgradeTower;
            towerInfoUI.OnDelete += SellTower;
            towerInfoUI.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _inputManager.OnClicked -= OnClicked;
            towerInfoUI.OnUpgrade -= UpgradeTower;
            towerInfoUI.OnDelete -= SellTower;
        }

        private void OnClicked()
        {
            if (_inputManager.IsMouseOverUI()) return;

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit =
                Physics2D.Raycast(ray.origin, ray.direction, 10, towerLayerMask);

            if (IsRaycastHitAnything()
                && IsRaycastHitTower(out Tower tower)
                && tower != _selectedTower)
            {
                SetSelectedTower(tower);
            }
            else
            {
                ClearSelectedTower();
            }

            bool IsRaycastHitAnything()
            {
                return hit.collider != null;
            }

            bool IsRaycastHitTower(out Tower tower)
            {
                tower = hit.collider.GetComponentInChildren<Tower>();
                return tower != null;
            }
        }

        private void SetSelectedTower(Tower tower)
        {
            _selectedTower = tower;

            towerInfoUI.gameObject.SetActive(true);
            towerInfoUI.transform.position = tower.transform.position + (Vector3)towerInfoOffset;
        }

        private void ClearSelectedTower()
        {
            _selectedTower = null;

            towerInfoUI.gameObject.SetActive(false);
        }

        private void UpgradeTower()
        {
            _towersController.DoUpgrade(_selectedTower);
        }

        private void SellTower()
        {
            _towersController.DoSell(_selectedTower);
            ClearSelectedTower();
        }
    }
}