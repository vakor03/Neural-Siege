using System;
using _Project.Scripts.Core.Towers;

namespace _Project.Scripts.Algorithms.GA.Structs
{
    [Serializable]
    public class TowerStatsGA
    {
        public TowerTypeSO TowerType;
        public float DamagePerSecond;
        public float SlowingFactor;
        public bool IsAoe;
        public float Range;
        public float TimeShooting;
    }
}