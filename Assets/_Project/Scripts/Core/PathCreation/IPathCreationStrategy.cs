using System;
using _Project.Scripts.Core.Configs;
using UnityEngine;

namespace _Project.Scripts.Core.PathCreation
{
    public interface IPathCreationStrategy
    {
        event Action<Vector3[]> OnPathCreated;
        void StartCreatingPath();
        void Initialize(EnemyPathConfigSO config);
    }
}