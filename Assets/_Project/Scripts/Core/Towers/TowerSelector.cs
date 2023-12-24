using _Project.Scripts.Core.Managers;
using _Project.Scripts.Core.UI;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Core.Towers
{
    public class TowerSelector : MonoBehaviour
    {
        [SerializeField] private LayerMask towerLayerMask;
        [SerializeField] private TowerInfoUI towerInfoUI;
        [SerializeField] private Vector2 towerInfoOffset;
        [SerializeField] private LineRenderer lineRenderer;
        
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

            _towersController.OnTowerUpgraded += SetSelectedTower;
        }

        private void OnDestroy()
        {
            _inputManager.OnClicked -= OnClicked;
            towerInfoUI.OnUpgrade -= UpgradeTower;
            towerInfoUI.OnDelete -= SellTower;
            
            _towersController.OnTowerUpgraded -= SetSelectedTower;
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
            DrawCircle(tower.gameObject.transform.position, tower.Range, lineRenderer);
        }
        
        void DrawCircle(Vector2 center, float radius, LineRenderer lineRenderer)
        {
            int resolution = 100; // Increase for a more accurate circle
            lineRenderer.positionCount = resolution + 1;

            for (int i = 0; i <= resolution; i++)
            {
                float angle = Mathf.PI * 2 * i / resolution;
                Vector2 newPoint = center + new Vector2(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
                lineRenderer.SetPosition(i, new Vector3(newPoint.x, newPoint.y, 0));
            }
        }

        private void ClearSelectedTower()
        {
            _selectedTower = null;

            towerInfoUI.gameObject.SetActive(false);
            lineRenderer.positionCount = 0;
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