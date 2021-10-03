using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BaksoGame
{
    public class WarungUI : MonoBehaviour
    {

        public GameObject warungListTransform;
        public WarungIngredientButton warungIngredientPrefab;
        public Text netBuyText;
        public Color notEnoughColor;
        public Color normalColor;

        private List<WarungIngredientButton> allWarungButtons = new List<WarungIngredientButton>();



        public void InitiateWarung()
        {
            foreach (var button in allWarungButtons)
            {
                Destroy(button.gameObject);
            }

            allWarungButtons.Clear();

            var allIngredients = ConsoleBaksoMain.Instance.allIngredients;

            foreach (var ingredient in allIngredients)
            {
                var newButton = Instantiate(warungIngredientPrefab, warungListTransform.transform);
                newButton.transform.localPosition = Vector3.zero;
                newButton.transform.localScale = Vector3.one;
                newButton.ingredientImage.sprite = ingredient.sprite;
                newButton.ID = ingredient.ingredientName;
                newButton.value_IngredientLeft.text = "x" + ConsoleBaksoMain.Instance.GetAmountIngredientLeft(ingredient.ingredientName).ToString();
                newButton.gameObject.SetActive(true);

                allWarungButtons.Add(newButton);
            }
        }

        private void Update()
        {
            int netPrice = ConsoleBaksoMain.Instance.totalMoney - TotalExpenses();

            if (netPrice < 0)
            {
                netBuyText.color = notEnoughColor;
            }
            else
            {
                netBuyText.color = normalColor;

            }

            netBuyText.text  = $"RP {netPrice.ToString("N0")} ";
        }

        private int TotalExpenses()
        {
            int price = 0;
            foreach(var ingredientButton in allWarungButtons)
            {
                price += ingredientButton.TotalPrice();
            }
            return price;
        }

        public void EndDay()
        {
            if (ConsoleBaksoMain.Instance.totalMoney - TotalExpenses() < 0)
            {
                return;
            }

            foreach(var button in allWarungButtons)
            {
                ConsoleBaksoMain.Instance.AddIngredientLeft(button.ID, button.GetTotalAmount());
            }

            ConsoleBaksoMain.Instance.totalMoney -= TotalExpenses();
            BaksoMainUI.instance.LetsGo();
        }

    }
}