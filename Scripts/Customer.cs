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

        [Space]
        public GameObject reaction_Neutral;
        public GameObject reaction_Good;
        public GameObject reaction_Bad;

        private float timer = 0.2f;

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
                moneyEarned = 35000;
            }
            else if (mistakes >= 1 && mistakes <= 2)
            {
                reaction_Neutral.gameObject.SetActive(true);
                moneyEarned = 15000;
            }
            else if (mistakes >= 3)
            {
                reaction_Bad.gameObject.SetActive(true);
                moneyEarned = 5000;
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
            timer = 0.2f;
        }

        private void Update()
        {

            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                selectedHighlight.SetActive(false);
            }
        }
    }
}