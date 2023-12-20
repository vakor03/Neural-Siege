using System.Collections.Generic;
using _Project.Scripts.Core.Towers;

namespace _Project.Scripts.Algorithms.GA
{
    public struct TileStats
    {
        public float Distance;
        public List<TowerTypeSO> Towers;

        public TileStats(float distance, List<TowerTypeSO> towers)
        {
            Distance = distance;
            Towers = towers;
        }
    }
}