using _Project.Scripts.Core.Towers.TowerStats;
using _Project.Scripts.Core.Towers.TowerUpgrades;
using _Project.Scripts.Extensions;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public abstract class Tower : MonoBehaviour
    {
        [field: SerializeField] public TowerTypeSO TowerTypeSO { get; private set; }
        public int UpgradeLevel { get; protected set; }

        public virtual void ApplyUpgrade(TowerUpgradeSO towerUpgradeSO)
        {
            UpgradeLevel++;
        }
    }

    public abstract class Tower<TSelf, TStats> : Tower where TSelf : Tower where TStats : TowerStats<TSelf>
    {
        [SerializeField] protected TowerStatsSO<TSelf, TStats> towerStatsSO;
        protected TowerStatsController<TSelf, TStats> TowerStatsController;

        protected CircleCollider2D TowerCollider2D { get; private set; }

        public override void ApplyUpgrade(TowerUpgradeSO towerUpgradeSO)
        {
            base.ApplyUpgrade(towerUpgradeSO);
            TowerStatsController.ApplyUpgrade(towerUpgradeSO as TowerUpgradeSO<TSelf, TStats>);
        }

        protected void SetupCollider<T>(TowerStats<T> towerStats) where T : Tower
        {
            TowerCollider2D = gameObject.GetOrAdd<CircleCollider2D>();
            TowerCollider2D.radius = towerStats.Range;
            TowerCollider2D.isTrigger = true;

            var rigidbody2D = gameObject.GetOrAdd<Rigidbody2D>();
            rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }

        protected virtual void Awake()
        {
            InitTowerStats();
        }

        private void InitTowerStats()
        {
            TowerStatsController = new(towerStatsSO);
        }

        protected virtual void OnEnable()
        {
            TowerStatsController.OnStatsChanged += OnStatsChanged;
        }

        protected abstract void OnStatsChanged();

        protected virtual void OnDisable()
        {
            TowerStatsController.OnStatsChanged -= OnStatsChanged;
        }
    }
}