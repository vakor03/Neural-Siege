using System;
using _Project.Scripts.Core.Enemies;

namespace _Project.Scripts.Algorithms.GA.Structs
{
    [Serializable]
    public struct EnemyStatsGA
    {
        public EnemyType EnemyType;
        public float MaxHealth;
        public float Speed;
        public float ReproductionRate;
        public EnemyType SpawnedType;
        public float Price;
    }
}