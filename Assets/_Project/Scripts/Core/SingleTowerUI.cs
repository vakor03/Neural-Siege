using System;
using _Project.Scripts.Core.GridSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Core
{
    public class SingleTowerUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text towerNameText;
        [SerializeField] private TMP_Text towerPriceText;
        [SerializeField] private Image towerIcon;
        [SerializeField] private Button button;
        
        public event Action<PlacingObjectSO> OnButtonClicked;

        public PlacingObjectSO PlacingObjectSO { get; private set; }

        private void Awake()
        {
            button.onClick.AddListener(InvokeOnButtonClicked);
        }

        private void InvokeOnButtonClicked()
        {
            OnButtonClicked?.Invoke(PlacingObjectSO);
        }
        
        private void OnDestroy()
        {
            button.onClick.RemoveListener(InvokeOnButtonClicked);
        }


        public void Setup(PlacingObjectSO placingObjectSO)
        {
            PlacingObjectSO = placingObjectSO;
            towerNameText.text = placingObjectSO.name;
            towerPriceText.text = $"Price: {placingObjectSO.price}";
            towerIcon.sprite = placingObjectSO.icon;
        }
    }
}