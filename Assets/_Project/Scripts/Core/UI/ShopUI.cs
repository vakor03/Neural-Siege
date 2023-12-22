﻿using KBCore.Refs;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Core.UI
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text moneyAmountText;
        
        private Shop _shop;
        
        [Inject]
        private void Construct(Shop shop)
        {
            _shop = shop;
        }
        
        private void OnValidate()
        {
            this.ValidateRefs();
        }

        private void Start()
        {
            _shop.OnMoneyAmountChanged += UpdateMoneyAmountText;
            UpdateMoneyAmountText();
        }

        private void UpdateMoneyAmountText()
        {
            moneyAmountText.text = _shop.MoneyAmount.ToString();
        }
        
        private void OnDestroy()
        {
            _shop.OnMoneyAmountChanged -= UpdateMoneyAmountText;
        }
    }
}