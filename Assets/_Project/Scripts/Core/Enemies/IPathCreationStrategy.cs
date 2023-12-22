using System;
using _Project.Scripts.Infrastructure.States;
using UnityEngine;

namespace _Project.Scripts.Core.Enemies
{
    public interface IPathCreationStrategy
    {
        event Action<Vector3[]> OnPathCreated;
        void StartCreatingPath();
        void Initialize(EnemyPathConfigSO config);
    }
}