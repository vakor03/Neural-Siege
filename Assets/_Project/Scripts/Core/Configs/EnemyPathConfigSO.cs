using _Project.Scripts.Core.GridSystem;
using UnityEngine;

namespace _Project.Scripts.Core.Configs
{
    [CreateAssetMenu(menuName = "Configs/EnemyPathConfig", fileName = "EnemyPathConfigSO", order = 0)]
    public class EnemyPathConfigSO : ScriptableObject
    {
        public PlacementSystemObjectSO enemyTileSO;
        public PlacementSystemObjectSO startTileSO;
        public PlacementSystemObjectSO finishTileSO;
    }
}