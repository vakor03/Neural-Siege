namespace _Project.Scripts.Algorithms.GA
{
    public struct TowerStats
    {
        public float DamagePerSecond;
        public float SlowingFactor;
        public bool IsAoe;

        public TowerStats(float damagePerSecond, float slowingFactor, bool isAoe)
        {
            DamagePerSecond = damagePerSecond;
            SlowingFactor = slowingFactor;
            IsAoe = isAoe;
        }
    }
}