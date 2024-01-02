using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Scripts.GridEditor
{
    public class GridEditorTest : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private TileBase tileBase;
        private Camera camera;

        private void Awake()
        {
            camera = Camera.main;
        }
        private void Update()
        {
            var mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            var tilePosition = tilemap.WorldToCell(mousePosition);
            
            if (Input.GetMouseButtonDown(0))
            {
                SetTile(tilePosition);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                DeleteTile(tilePosition);
            }
        }

        public void SetTile(Vector3Int position)
        {
            tilemap.SetTile(position, tileBase);
        }

        public void DeleteTile(Vector3Int position)
        {
            tilemap.SetTile(position, null);
        }
    }
}