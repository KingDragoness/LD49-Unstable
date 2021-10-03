using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace BaksoGame
{
    public class IngredientPutButton : MonoBehaviour
    {

        public string itemID = "";
        public Text amountLeft;
        public Image ingredientImage;

        public void ShowHighlight()
        {
            string s = itemID;

            TooltipUI.Instance().AssignText(s);
        }

        public void HideHighlight()
        {
            TooltipUI.Instance().AssignText("");
        }
    }
}