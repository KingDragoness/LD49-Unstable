using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace BaksoGame
{

    public class TooltipUI : MonoBehaviour
    {
        public Text tooltipText;

        public void AssignText(string content)
        {
            tooltipText.text = content;
        }

        public static TooltipUI Instance()
        {
            return BaksoMainUI.instance.tooltipUI;
        }
    }
}