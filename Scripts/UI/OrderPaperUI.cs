using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BaksoGame
{
    public class OrderPaperUI : MonoBehaviour
    {

        public Text orderanText;
        public Text timerText;

        public void AssignText(string content)
        {
            orderanText.text = content;
        }

        public static OrderPaperUI Instance()
        {
            return BaksoMainUI.instance.orderPaperUI;
        }

        private void Update()
        {
            if (ConsoleBaksoMain.Instance.lastCustomerOrder != null)
            {
                var customer = ConsoleBaksoMain.Instance.lastCustomerOrder;

                timerText.text = $"{customer.TimerGame.ToString("0.00")} s";

            }

        }

    }
}