using _Project.Scripts.Core.GridSystem;
using KBCore.Refs;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Core.UI
{
    public class ChooseTowerUI : MonoBehaviour
    {
        [SerializeField] private Transform optionsParent;
        [SerializeField] private SingleTowerUI singleTowerUIPrefab;
        [SerializeField] private PlacementSystemObjectSO[] options;
        private PlacementSystem _placementSystem;
        
        private void OnValidate()
        {
            this.ValidateRefs();
        }

        [Inject]
        private void Construct(PlacementSystem placementSystem)
        {
            _placementSystem = placementSystem;
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
            _placementSystem.StartPlacingObject(placingObjectSO);
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