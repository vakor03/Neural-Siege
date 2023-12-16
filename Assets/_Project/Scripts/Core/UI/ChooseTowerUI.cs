using _Project.Scripts.Core.GridSystem;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts.Core.UI
{
    public class ChooseTowerUI : MonoBehaviour
    {
        [SerializeField] private Transform optionsParent;
        [SerializeField] private SingleTowerUI singleTowerUIPrefab;
        [SerializeField] private PlacementSystemObjectSO[] options;
        [SerializeField, Scene] private PlacementSystem placementSystem;
        
        private void OnValidate()
        {
            this.ValidateRefs();
        }
        
        private void Start()
        {
            foreach (Transform child in optionsParent)
            {
                Destroy(child.gameObject);
            }
            foreach (var option in options)
            {
                var singleTowerUI = Instantiate(singleTowerUIPrefab, optionsParent);
                singleTowerUI.Setup(option);
                singleTowerUI.OnButtonClicked += PlaceBuilding;
            }
        }

        private void PlaceBuilding(PlacementSystemObjectSO placingObjectSO)
        {
            placementSystem.StartPlacingObject(placingObjectSO);
        }
        
        private void OnDestroy()
        {
            foreach (Transform child in optionsParent)
            {
                var singleTowerUI = child.GetComponent<SingleTowerUI>();
                singleTowerUI.OnButtonClicked -= PlaceBuilding;
            }
        }
    }
}