using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaksoGame
{

    public class Customer : MonoBehaviour
    {

        public GameObject orderExclamation;
        public Sprite NPCSprite;
        public GameObject selectedHighlight;
        public List<IngredientForm> needsIngredients = new List<IngredientForm>();
        public bool hasBeenServed = false;
        public Vector2 timerPatient = new Vector2(15,30);

        [Space]
        public int rewardBase = 40000;
        public bool isPissedOff = false;

        [Space]
        public GameObject reaction_Neutral;
        public GameObject reaction_Good;
        public GameObject reaction_Bad;

        private float highlightTimer = 0.2f;
        private float timerGame = 30f;
        private bool startOrder = false;

        public float TimerGame { get => timerGame; set => timerGame = value; }
        public bool HasStartedOrder { get => startOrder;  }

        private void Start()
        {
            selectedHighlight.SetActive(false);
            reaction_Neutral.SetActive(false);
            reaction_Good.SetActive(false);
            reaction_Bad.SetActive(false);

            CreateIngredientDemand();
        }

        private void CreateIngredientDemand()
        {
            foreach(var ingredient in ConsoleBaksoMain.Instance.allIngredients)
            {
                float randomTime = Random.Range(0, 1f);
                int amount = Mathf.RoundToInt(ingredient.randomDistribution.Evaluate(randomTime));

                if (amount <= 0)
                {
                    continue;
                }

                IngredientForm form = new IngredientForm();
                form.ID = ingredient.ingredientName;
                form.amount = amount;

                needsIngredients.Add(form);
            }
        }

        public void FoodServed(BowlCooked bowlTarget)
        {
            hasBeenServed = true;
            int moneyEarned = 10;

            //check errors
            int mistakes = 0;

            foreach(var myIngredient in needsIngredients)
            {
                var targetIngredient = bowlTarget.allIngredients.Find(x => x.ID == myIngredient.ID);

                if (targetIngredient == null)
                {
                    mistakes = 2;
                }
                else
                {
                    if (targetIngredient.amount < myIngredient.amount)
                    {
                        mistakes++;
                    }
                }
            }

            if (mistakes <= 0)
            {
                reaction_Good.gameObject.SetActive(true);
                moneyEarned = rewardBase;
            }
            else if (mistakes >= 1 && mistakes <= 2)
            {
                reaction_Neutral.gameObject.SetActive(true);
                moneyEarned = rewardBase / 2;
            }
            else if (mistakes >= 3)
            {
                reaction_Bad.gameObject.SetActive(true);
                moneyEarned = rewardBase / 5;
                isPissedOff = true;

            }

            ConsoleBaksoMain.Instance.todayMoneyEarned += moneyEarned;
            ConsoleBaksoMain.Instance.totalMoney += moneyEarned;
            TooltipUI.Instance().AssignText($"Earned: Rp {moneyEarned}");
            orderExclamation.SetActive(false);
        }

        public void ToggleExclamation(bool enable = false)
        {
            orderExclamation.SetActive(enable);
        }

        public void SelectHighlight()
        {
            selectedHighlight.SetActive(true);
            highlightTimer = 0.2f;
        }

        public void StartTimer()
        {

            if (startOrder == false)
                TimerGame = Random.Range(timerPatient.x, timerPatient.y);

            startOrder = true;

        }

        private void Update()
        {

            if (highlightTimer > 0)
            {
                highlightTimer -= Time.deltaTime;
            }
            else
            {
                selectedHighlight.SetActive(false);
            }

            if (startOrder && hasBeenServed == false)
            {
                if (TimerGame > 0)
                {
                    TimerGame -= Time.deltaTime;
                }
                else
                {
                    hasBeenServed = true;
                    selectedHighlight.SetActive(false);
                    reaction_Bad.gameObject.SetActive(true);
                    orderExclamation.SetActive(false);
                    isPissedOff = true;
                }
            }
        }
    }
}