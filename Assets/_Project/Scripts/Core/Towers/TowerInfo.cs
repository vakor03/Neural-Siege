using _Project.Scripts.Core.GridSystem;
using _Project.Scripts.Core.Towers.TowerUpgrades;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    [CreateAssetMenu(menuName = "Create TowerInfo", fileName = "TowerInfo", order = 0)]
    public class TowerInfo : ScriptableObject
    {
        public TowerType type;
        public PlacementSystemObjectSO placementObject;
        public TowerUpgradeSO upgrade;
    }
}