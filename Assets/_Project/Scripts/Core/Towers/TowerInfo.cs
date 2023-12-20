using _Project.Scripts.Core.GridSystem;
using _Project.Scripts.Core.Towers.TowerUpgrades;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Core.Towers
{
    [CreateAssetMenu(menuName = "Create TowerInfo", fileName = "TowerInfo", order = 0)]
    public class TowerInfo : ScriptableObject
    {
        [FormerlySerializedAs("type")] public TowerTypeSO typeSO;
        public PlacementSystemObjectSO placementObject;
        public TowerUpgradeSO upgrade;
    }
}