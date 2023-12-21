using _Project.Scripts.Core.Towers;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Core.UI
{
    public class ChooseTowerUI : MonoBehaviour
    {
        [SerializeField] private Transform optionsParent;
        [SerializeField] private SingleTowerUI singleTowerUIPrefab;
        [SerializeField] private TowerInfoSO[] options;
        
        private TowersController _towersController;

        [Inject]
        private void Construct(TowersController towersController)
        {
            _towersController = towersController;
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

        private void PlaceBuilding(TowerInfoSO placingObjectSO)
        {
            _towersController.StartPlacement(placingObjectSO.typeSO);
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