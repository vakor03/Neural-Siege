using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    [CreateAssetMenu(menuName = "TowerStatsSO/FreezingTower", fileName = "FreezingTowerStatsSO", order = 0)]
    public class FreezingTowerStatsSO : TowerStatsSO
    {
        public float freezingMultiplier = 0.5f;
    }
}