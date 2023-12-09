﻿using System;
using _Project.Scripts.Extensions;
using JetBrains.Annotations;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts
{
    public class SimpleTower : MonoBehaviour
    {
        [SerializeField] private float range;
        [SerializeField, Self] private RotateTowards rotateTowards;
        [SerializeField, Self] private TargetChooseStrategy targetChooseStrategy;

        [SerializeField] private Transform shootPosition;
        
        [SerializeField] private float attackInterval = 1f;
        [SerializeField] private Projectile projectilePrefab;
        

        private ITimer _timer;


        [CanBeNull] private Enemy _activeTarget;

        private void OnValidate()
        {
            this.ValidateRefs();
        }

        private void Awake()
        {
            _timer = new TickTimer();
            _timer.Duration = attackInterval;
            _timer.OnTimeElapsed += OnTimeElapsed;
            SetupCollider();
            targetChooseStrategy.Range = range;
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
            if (_activeTarget == null) return;

            ShootEnemy(_activeTarget);
        }

        private void ShootEnemy(Enemy enemy)
        {
            Projectile projectile = Instantiate(projectilePrefab, shootPosition.position, Quaternion.identity);
            projectile.SetTarget(enemy.transform);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_activeTarget == null &&
                other.gameObject.TryGetComponent<Enemy>(out var enemy))
            {
                ChangeActiveTarget(enemy);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_activeTarget != null &&
                other.gameObject == _activeTarget.gameObject)
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
            _activeTarget = enemy;
            rotateTowards.Target = _activeTarget!.transform;
        }

        private void RemoveActiveTarget()
        {
            _activeTarget = null;
            rotateTowards.Target = null;
        }

        private void OnDrawGizmos()
        {
            if (_activeTarget == null)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.green;
            }

            Gizmos.DrawWireSphere(transform.position, range);
        }

        private void SetupCollider()
        {
            var collider2D = gameObject.GetOrAdd<CircleCollider2D>();
            collider2D.radius = range;
            collider2D.isTrigger = true;

            var rigidbody2D = gameObject.GetOrAdd<Rigidbody2D>();
            rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}