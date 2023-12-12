using JetBrains.Annotations;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public abstract class SingleTargetTower : Tower
    {
        [SerializeField, Self] protected RotateTowards rotateTowards;
        [SerializeField, Self] protected TargetChooseStrategy targetChooseStrategy;
        [CanBeNull] protected Enemy ActiveTarget;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (ActiveTarget == null &&
                other.gameObject.TryGetComponent<Enemy>(out var enemy))
            {
                ChangeActiveTarget(enemy);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (ActiveTarget != null &&
                other.gameObject == ActiveTarget.gameObject)
            {
                RemoveActiveTarget();
                if (targetChooseStrategy.TryChooseNewTarget(out var enemy))
                {
                    ChangeActiveTarget(enemy);
                }
            }
        }
        
        private void ChangeActiveTarget(Enemy enemy)
        {
            ActiveTarget = enemy;
            rotateTowards.Target = ActiveTarget!.transform;
        }

        private void RemoveActiveTarget()
        {
            ActiveTarget = null;
            rotateTowards.Target = null;
        }
    }
}