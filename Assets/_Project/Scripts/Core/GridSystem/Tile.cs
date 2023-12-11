using UnityEngine;

namespace _Project.Scripts.Core.GridSystem
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private GameObject availableVisual;
        [SerializeField] private GameObject unavailableVisual;
        
        private bool _isAvailable;

        public bool IsAvailable
        {
            get => _isAvailable;
            set
            {
                if (_isAvailable == value)
                {
                    return;
                }

                _isAvailable = value;
                availableVisual.SetActive(_isAvailable);
                unavailableVisual.SetActive(!_isAvailable);
            }
        }
        
        private void Awake()
        {
            IsAvailable = true;
        }

        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}