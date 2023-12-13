using _Project.Scripts.Core.GridSystem;
using KBCore.Refs;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public class ChooseTowerUI : MonoBehaviour
    {
        [SerializeField] private Transform optionsParent;
        [SerializeField] private SingleTowerUI singleTowerUIPrefab;
        [SerializeField] private PlacingObjectSO[] options;
        
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

        private void PlaceBuilding(PlacingObjectSO placingObjectSO)
        {
            ObjectPlacer.Instance.SetPlacingObject(placingObjectSO);
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