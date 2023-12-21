﻿using System;
using _Project.Scripts.Core.Towers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Core.UI
{
    public class SingleTowerUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text towerNameText;
        [SerializeField] private TMP_Text towerPriceText;
        [SerializeField] private Image towerIcon;
        [SerializeField] private Button button;
        
        public event Action<TowerInfoSO> OnButtonClicked;

        public TowerInfoSO PlacingObjectSO { get; private set; }

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


        public void Setup(TowerInfoSO placingObjectSO)
        {
            PlacingObjectSO = placingObjectSO;
            towerNameText.text = placingObjectSO.name;
            towerPriceText.text = $"Price: {placingObjectSO.price}";
            towerIcon.sprite = placingObjectSO.placementObject.icon;
        }
    }
}