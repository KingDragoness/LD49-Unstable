using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BaksoGame
{
    public class BowlButton : MonoBehaviour
    {

        public BowlCooked bowl;

        public void ShowHighlight()
        {
            string s = "";

            foreach(var ingredient in bowl.allIngredients)
            {
                s += $"{ingredient.amount} {ingredient.ID}, ";
            }

            TooltipUI.Instance().AssignText(s);
        }

        public void HideHighlight()
        {
            TooltipUI.Instance().AssignText("");
        }

    }
}