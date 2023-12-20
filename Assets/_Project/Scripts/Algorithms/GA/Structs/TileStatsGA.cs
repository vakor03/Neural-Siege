using System.Collections.Generic;
using _Project.Scripts.Core.Towers;

namespace _Project.Scripts.Algorithms.GA.Structs
{
    public struct TileStatsGA
    {
        public float Distance;
        public List<TowerTypeSO> Towers;

        public TileStatsGA(float distance, List<TowerTypeSO> towers)
        {
            Distance = distance;
            Towers = towers;
        }
    }
}