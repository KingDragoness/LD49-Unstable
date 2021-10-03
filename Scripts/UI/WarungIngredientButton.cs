using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BaksoGame
{

    public class WarungIngredientButton : MonoBehaviour
    {

        public string ID = "";
        public Text value_IngredientLeft;
        public InputField inputField_BuyAmount;
        public Text value_TotalPrice;
        public Image ingredientImage;

        public void OnValueChange()
        {
            int buyAmount = int.Parse(inputField_BuyAmount.text);
            int totalCost = ConsoleBaksoMain.Instance.GetIngredient(ID).priceBuy * buyAmount;

            value_TotalPrice.text = $"RP {totalCost.ToString("N0")} ";
        }

        public int GetTotalAmount()
        {
            int buyAmount = int.Parse(inputField_BuyAmount.text);

            return buyAmount;
        }

        public int TotalPrice()
        {
            int buyAmount = int.Parse(inputField_BuyAmount.text);
            int totalCost = ConsoleBaksoMain.Instance.GetIngredient(ID).priceBuy * buyAmount;
            return totalCost;
        }

    }
}