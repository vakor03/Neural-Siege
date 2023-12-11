using UnityEngine;

namespace _Project.Scripts.Core.GridSystem
{
    public class TestGrid : MonoBehaviour
    {
        [SerializeField] private Tile tilePrefab;
        [SerializeField] private GridSetupSO gridSetupSO;
        [SerializeField] private ObjectPlacer objectPlacer;
        [SerializeField] private GridEditor gridEditor;

        private Grid<Tile> _grid;

        private void Start()
        {
            LoadGrid();
            EnableObjectPlacer();
        }

        [ContextMenu("SaveGrid")]
        private void SaveGrid()
        {
            gridSetupSO.Save(_grid);
        }

        [ContextMenu("LoadGrid")]
        private void LoadGrid()
        {
            _grid = gridSetupSO.Load(tilePrefab, transform);
            objectPlacer.Initialize(_grid, transform);
            gridEditor.Initialize(_grid, tilePrefab);
        }

        [ContextMenu("EnableGridEditor")]
        private void EnableGridEditor()
        {
            gridEditor.enabled = true;
            objectPlacer.enabled = false;
        }

        [ContextMenu("EnableObjectPlacer")]
        private void EnableObjectPlacer()
        {
            gridEditor.enabled = false;
            objectPlacer.enabled = true;
        }
    }
}