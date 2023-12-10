using UnityEngine;

namespace _Project.Scripts
{
    [CreateAssetMenu(menuName = "Create GridSetup", fileName = "GridSetup", order = 0)]
    public class GridSetup : ScriptableObject
    {
        public bool?[,] grid;
    }
}