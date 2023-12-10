using TMPro;
using UnityEngine;

namespace _Project.Scripts
{
    public class Grid<T>
    {
        private readonly int _width;
        private readonly int _height;
        private readonly T[] _grid;

        private float _cellSize;
        private Vector3 _originPosition;

        public int Width => _width;

        public int Height => _height;

        public T[] Grid1 => _grid;

        public float CellSize => _cellSize;

        public Vector3 OriginPosition => _originPosition;

        private readonly IGridCoordinateConverter _coordinateConverter = new HorizontalConverter();

        public Grid(int width, int height, float cellSize, Vector3 origin, bool debug)
        {
            _width = width;
            _height = height;
            _grid = new T[width * height];
            _cellSize = cellSize;
            _originPosition = origin;

            if (debug)
            {
                DrawDebugLines();
            }
        }

        public T this[int x, int y]
        {
            get => _grid[x + y * _width];
            set => _grid[x + y * _width] = value;
        }

        public T this[Vector2Int position]
        {
            get => this[position.x, position.y];
            set => this[position.x, position.y] = value;
        }

        private void DrawDebugLines()
        {
            const float duration = 100f;
            var parent = new GameObject("Debugging");

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    CreateWorldText(parent, x + "," + y, GetWorldPositionCentered(x, y), _coordinateConverter.Forward);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, duration);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, duration);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, _height), GetWorldPosition(_width, _height), Color.white, duration);
            Debug.DrawLine(GetWorldPosition(_width, 0), GetWorldPosition(_width, _height), Color.white, duration);
        }

        public Vector3 GetWorldPosition(int x, int y)
        {
            return _coordinateConverter.GridToWorld(new Vector2Int(x, y), _cellSize, _originPosition);
        }

        public Vector3 GetWorldPositionCentered(int x, int y)
        {
            return _coordinateConverter.GridToWorldCentered(new Vector2Int(x, y), _cellSize, _originPosition);
        }
        
        public Vector2Int GetGridPosition(Vector3 worldPosition)
        {
            return _coordinateConverter.WorldToGrid(worldPosition, _cellSize, _originPosition);
        }

        private TextMeshPro CreateWorldText(GameObject parent, string text, Vector3 position, Vector3 dir,
            int fontSize = 2, Color color = default, TextAlignmentOptions textAnchor = TextAlignmentOptions.Center,
            int sortingOrder = 0)
        {
            GameObject gameObject = new GameObject("DebugText_" + text, typeof(TextMeshPro));
            gameObject.transform.SetParent(parent.transform);
            gameObject.transform.position = position;
            gameObject.transform.forward = dir;

            TextMeshPro textMeshPro = gameObject.GetComponent<TextMeshPro>();
            textMeshPro.text = text;
            textMeshPro.fontSize = fontSize;
            textMeshPro.color = color == default ? Color.white : color;
            textMeshPro.alignment = textAnchor;
            textMeshPro.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

            return textMeshPro;
        }
    }
}