using _Project.Scripts.Core.GridSystem;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public class EnemyPathCreator : MonoBehaviour
    {
        [SerializeField] private PlacementSystem placementSystem = new();
        [SerializeField] private PlacementSystemObjectSO enemyTileSO;   
        
        private BacktrackingPathGenerator _pathGenerator = new();

        [ContextMenu("Create Path")]
        public void CreatePath()
        {
            Vector2Int gridSize = placementSystem.Size;
            (Vector2Int start, Vector2Int finish) = GetStartAndFinish(gridSize);
            var path = _pathGenerator.GeneratePath(gridSize, start, finish);
            
            foreach (Vector3Int position in path)
            {
                placementSystem.BuildObject(position + placementSystem.BottomLeft, enemyTileSO);
            }
        }

        private (Vector2Int start, Vector2Int finish) GetStartAndFinish(Vector2Int size)
        {
            return (new Vector2Int(0, 0), new Vector2Int(8, 8));
        }
    }
}