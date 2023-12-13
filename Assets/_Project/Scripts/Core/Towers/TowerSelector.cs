using _Project.Scripts.Core.GridSystem;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts.Core.Towers
{
    public class TowerSelector : MonoBehaviour
    {
        [SerializeField, Self] private BoxCollider2D boxCollider2D;
        [SerializeField] private TowerInfoUI towerInfoUI;
        [SerializeField] private TowerType type;
        
        private void OnValidate()
        {
            this.ValidateRefs();
        }

        public void ToggleTowerUIInfo()
        {
            if (!towerInfoUI.gameObject.activeSelf)
            {
                towerInfoUI.gameObject.SetActive(true);
            }
        }
    }
}