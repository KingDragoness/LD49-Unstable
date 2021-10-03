using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BaksoGame
{
    public class BaksoMainUI : MonoBehaviour
    {

        public GameObject canvas_0;
        public GameObject canvas_1;
        public GameObject canvas_2;
        public Animator animator_CanvasGroup_0;
        public Canvas mainCanvas;
        [Space]
        public TooltipUI tooltipUI;
        public OrderPaperUI orderPaperUI;
        public WarungUI warungUI;
        [Space]
        public Text moneyText;
        public Text dayText;
        public Slider sliderHP;
        public GameObject gameoverScreen;
        [Space]
        [Header("End of Report")]
        public GameObject reportSession;
        public GameObject afterDaySession;
        public Text value_TodayMoneyEarned;
        public Text value_DailyExpense;
        public Text label_DailyExpense;
        public Text value_MoneyLeft;
        public Text funnyText;
        public GameObject gameoverNoMoneyLeft;
        public GameObject normalButtonReport;
        [Space]
        public AudioSource failSFX;
        public AudioSource victorySFX;
        public AudioSource tingtingSFX;

        public static BaksoMainUI instance;

        //CAPEK

        private void Awake()
        {
            instance = this;
        }

        [ContextMenu("StartDay")]
        public void StartDay()
        {
            canvas_1.gameObject.SetActive(false);
            ConsoleBaksoMain.Instance.StartNewLevel();
            animator_CanvasGroup_0.SetTrigger("Show");
        }

        private void Update()
        {
            string uang = ConsoleBaksoMain.Instance.totalMoney.ToString("N0");
            moneyText.text = $"RP {uang} ";
            sliderHP.value = ConsoleBaksoMain.Instance.hitpointGerobak/100f;
            sliderHP.maxValue = 1;

            var dayNight = ConsoleBaksoMain.Instance.dayNightCycle;

            var minute = dayNight.GetCurrentTime() - System.Math.Truncate(dayNight.GetCurrentTime());
            var hour = Mathf.Floor(dayNight.GetCurrentTime());

            {

                minute = Mathf.Lerp(0, 60, (float)minute);
                minute = System.Math.Floor(minute);
            }

            string huehue = $"{hour}:{minute:00}";

            dayText.text = $"{huehue} | Day {ConsoleBaksoMain.Instance.day} ";

        }

        public void OpenReportSession()
        {
            string uang = ConsoleBaksoMain.Instance.totalMoney.ToString("N0");
            string uangEarned = ConsoleBaksoMain.Instance.todayMoneyEarned.ToString("N0");
            string expensesToday = ConsoleBaksoMain.Instance.TotalExpenses().ToString("N0");

            {
                string LabelString = "[";

                foreach (var expense in ConsoleBaksoMain.Instance.todayExpenses)
                {
                    LabelString += $"{expense}, ";
                }

                LabelString += "]";
                label_DailyExpense.text = LabelString;
            }

            if (ConsoleBaksoMain.Instance.totalMoney < 0)
            {
                gameoverNoMoneyLeft.gameObject.SetActive(true);
                normalButtonReport.gameObject.SetActive(false);
                failSFX.Play();
                funnyText.text = "My wife has abandoned me and my landlord kicked me out.";
            }
            else
            {
                victorySFX.Play();

                if (ConsoleBaksoMain.Instance.totalMoney < 10000)
                {
                    funnyText.text = "HOO! Close call...";
                }
                else if (ConsoleBaksoMain.Instance.totalMoney < 100000)
                {
                    funnyText.text = "This is bad.";
                }
                else if (ConsoleBaksoMain.Instance.totalMoney < 500000)
                {
                    funnyText.text = "Money is running out... Need to be smarter...";
                }
                else if (ConsoleBaksoMain.Instance.totalMoney < 2000000)
                {
                    funnyText.text = "At least I can survive this amount.";
                }
                else if (ConsoleBaksoMain.Instance.totalMoney < 6000000)
                {
                    funnyText.text = "Heh. Not bad.";
                }
                else if (ConsoleBaksoMain.Instance.totalMoney < 10000000)
                {
                    funnyText.text = "Now I can be comfortable...";
                }
                else if (ConsoleBaksoMain.Instance.totalMoney < 20000000)
                {
                    funnyText.text = "Nice... I AM RICH.";
                }
                else if (ConsoleBaksoMain.Instance.totalMoney >= 20000000)
                {
                    funnyText.text = "I proclaim the throne of Sultanate of Bakso Empire.";
                }
            }

            reportSession.gameObject.SetActive(true);
            afterDaySession.gameObject.SetActive(false);
            orderPaperUI.gameObject.SetActive(false);
            canvas_2.gameObject.SetActive(true);
            value_TodayMoneyEarned.text = $"RP {uangEarned} ";
            value_DailyExpense.text = $"RP {expensesToday} ";
            value_MoneyLeft.text = $"RP {uang} ";

        }

        public void OpenAfterdaySession()
        {
            reportSession.gameObject.SetActive(false);
            afterDaySession.gameObject.SetActive(true);
            canvas_2.gameObject.SetActive(true);
            warungUI.InitiateWarung();
        }

        public void LetsGo()
        {
            canvas_1.gameObject.SetActive(true);
            canvas_2.gameObject.SetActive(false);
        }
    }
}