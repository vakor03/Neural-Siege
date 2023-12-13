using _Project.Scripts.Core.Effects;
using _Project.Scripts.Core.Towers.TowerStats;
using _Project.Scripts.Core.Towers.TowerUpgrades;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public class FreezingTower : Tower
    {
        [SerializeField] private FreezingTowerStatsSO towerStatsSO;
        [SerializeField] private FreezingTowerUpgradeSO upgradeSO;
        
        private TowerStatsController<FreezingTower, FreezingTowerStats> _towerStatsController;

        private FreezeEffect _freezingEffect;

        private void Awake()
        {
            InitTowerStats();
            SetupCollider(_towerStatsController.CurrentStats);
            float freezingMultiplier = _towerStatsController.CurrentStats.FreezingMultiplier;
            _freezingEffect = new FreezeEffect(freezingMultiplier);
        }

        private void OnEnable()
        {
            _towerStatsController.OnStatsChanged += OnStatsChanged;
        }

        private void OnStatsChanged()
        {
            float freezingMultiplier = _towerStatsController.CurrentStats.FreezingMultiplier;
            float range = _towerStatsController.CurrentStats.Range;
            _freezingEffect.FreezeMultiplier = freezingMultiplier;
            TowerCollider2D.radius = range;
        }
      

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _towerStatsController.ApplyUpgrade(upgradeSO);
            }
        }

        private void OnDestroy()
        {
            _towerStatsController.OnStatsChanged -= OnStatsChanged;
        }

        private void InitTowerStats()
        {
            _towerStatsController = new(towerStatsSO);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.ApplyEffect(_freezingEffect);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.RemoveEffect(_freezingEffect);
            }
        }

        private void OnDrawGizmos()
        {
            float range = _towerStatsController.CurrentStats.Range;
            Gizmos.color = Color.green;

            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}