using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public class SimpleTower : SingleTargetTower
    {
        [SerializeField] private SimpleTowerStatsSO towerStatsSO;
        [SerializeField] private Transform shootPosition;

        private ITimer _timer;
        
        private void OnValidate()
        {
            this.ValidateRefs();
        }

        private void Awake()
        {
            _timer = new TickTimer();
            _timer.Duration = towerStatsSO.attackInterval;
            _timer.OnTimeElapsed += OnTimeElapsed;
            SetupCollider(towerStatsSO);
            targetChooseStrategy.Range = towerStatsSO.range;
        }

        private void Start()
        {
            _timer.Start();
        }

        private void OnDestroy()
        {
            _timer.OnTimeElapsed -= OnTimeElapsed;
            _timer.Stop();
        }

        private void OnTimeElapsed()
        {
            if (ActiveTarget == null) return;

            ShootEnemy(ActiveTarget);
        }

        private void ShootEnemy(Enemy enemy)
        {
            Projectile projectile =
                Instantiate(towerStatsSO.projectilePrefab, shootPosition.position, Quaternion.identity);
            projectile.SetTarget(enemy.transform);
        }

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

        private void OnDrawGizmos()
        {
            if (ActiveTarget == null)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.green;
            }

            Gizmos.DrawWireSphere(transform.position, towerStatsSO.range);
        }
    }
}