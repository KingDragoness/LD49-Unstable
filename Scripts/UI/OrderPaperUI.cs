using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BaksoGame
{
    public class OrderPaperUI : MonoBehaviour
    {

        public Text orderanText;

        public void AssignText(string content)
        {
            orderanText.text = content;
        }

        public static OrderPaperUI Instance()
        {
            return BaksoMainUI.instance.orderPaperUI;
        }

    }
}