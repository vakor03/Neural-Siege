using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    [CreateAssetMenu(menuName = "Create TowerInfoDatabaseSO", fileName = "TowerInfoDatabaseSO", order = 0)]
    public class TowerInfoDatabaseSO : ScriptableObject
    {
        public TowerInfoSO[] towerInfos;
    }
}