using KBCore.Refs;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text moneyAmountText;
        [SerializeField, Scene] private Shop shop;
        
        private void OnValidate()
        {
            this.ValidateRefs();
        }

        private void Start()
        {
            shop.OnMoneyAmountChanged += UpdateMoneyAmountText;
            UpdateMoneyAmountText();
        }

        private void UpdateMoneyAmountText()
        {
            moneyAmountText.text = shop.MoneyAmount.ToString();
        }
        
        private void OnDestroy()
        {
            shop.OnMoneyAmountChanged -= UpdateMoneyAmountText;
        }
    }
}