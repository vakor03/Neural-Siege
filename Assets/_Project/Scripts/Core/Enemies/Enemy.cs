using _Project.Scripts.Core.WaypointSystem;
using KBCore.Refs;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Core.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [field: SerializeField] public EnemyType EnemyType { get; private set; }
        [field: SerializeField, Self] public WaypointsMover WaypointsMover { get; private set; }
        [field: SerializeField, Self] public EnemyStatsSystem EnemyStatsSystem { get; private set; }
        [field: SerializeField, Self] public EnemyHealth EnemyHealth { get; private set; }

        private IPlayerBase _playerBase;

        [Inject]
        private void Construct(IPlayerBase playerBase)
        {
            _playerBase = playerBase;
        }

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        protected virtual void Start()
        {
            WaypointsMover.OnPathCompleted += OnPathCompleted;
        }

        protected virtual void OnDestroy()
        {
            WaypointsMover.OnPathCompleted -= OnPathCompleted;
        }

        private void OnPathCompleted()
        {
            _playerBase.TakeDamage(1);
            EnemyHealth.DestroySelf();
        }
    }
}