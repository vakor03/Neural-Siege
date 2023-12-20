using _Project.Scripts.Core.Enemies;

namespace _Project.Scripts.Algorithms.GA
{
    public struct EnemyStats
    {
        public float MaxHealth;
        public float Speed;
        public float ReproductionRate;
        public EnemyType SpawnedType;
        public float Price;

        public EnemyStats(float maxHealth, float speed, float reproductionRate, EnemyType spawnedType, float price)
        {
            MaxHealth = maxHealth;
            Speed = speed;
            ReproductionRate = reproductionRate;
            SpawnedType = spawnedType;
            Price = price;
        }
    }
}