using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BaksoGame
{

    [System.Serializable]
    public class BowlCooked
    {
        public List<IngredientForm> allIngredients = new List<IngredientForm>();
    }

    [System.Serializable]
    public class IngredientForm
    {
        public string ID = "";
        public int amount = 0;
    }


    public class OrderanUI : MonoBehaviour
    {

        public List<BowlCooked> allBowlCooked = new List<BowlCooked>();
        public List<IngredientForm> allIngredientsInPot = new List<IngredientForm>();
        public Text tooltipText;
        public BowlButton currentHoldBowl;

        [Space]
        public RectTransform ingredientButtonListTransform;
        public RectTransform bowlButtonListTransform;
        public IngredientPutButton ingredientButtonPrefab;
        public BowlButton bowlButtonPrefab;

        private List<IngredientPutButton> allIngredientButtons = new List<IngredientPutButton>();
        private List<BowlButton> allBowlButtons = new List<BowlButton>();

        public static OrderanUI instance;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            RefreshAllButtons();
            ingredientButtonPrefab.gameObject.SetActive(false);
            bowlButtonPrefab.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            if (Time.timeSinceLevelLoad > 1f)
                RefreshAllButtons();
        }

        private void RefreshAllButtons()
        {
            foreach(var button in allIngredientButtons)
            {
                Destroy(button.gameObject);
            }

            allIngredientButtons.Clear();

            var allIngredients = ConsoleBaksoMain.Instance.allIngredients;

            foreach(var ingredient in allIngredients)
            {
                var newButton = Instantiate(ingredientButtonPrefab, ingredientButtonListTransform.transform);
                newButton.transform.localPosition = Vector3.zero;
                newButton.transform.localScale = Vector3.one;
                newButton.ingredientImage.sprite = ingredient.sprite;
                newButton.itemID = ingredient.ingredientName;
                newButton.amountLeft.text = "x" + ConsoleBaksoMain.Instance.GetAmountIngredientLeft(ingredient.ingredientName).ToString();
                newButton.gameObject.SetActive(true);

                allIngredientButtons.Add(newButton);
            }
        }


        public void ThrowBowl(BowlButton bowl)
        {
            allBowlCooked.Remove(bowl.bowl);
            RefreshAllBowl();

        }

        public void Pot_Cook()
        {
            if (allIngredientsInPot.Count == 0)
            {
                BaksoMainUI.instance.tooltipUI.AssignText("No ingredient!");
                return;
            }

            CreateNewBowl(allIngredientsInPot);
            allIngredientsInPot.Clear();
            RefreshTooltip();
            RefreshAllBowl();
            //reset all ingredient
        }

        public void Pot_NewIngredient(string ID)
        {
            IngredientForm ingredient = null;
            var existingIngredient = allIngredientsInPot.Find(x => x.ID == ID);

            if (ConsoleBaksoMain.Instance.GetAmountIngredientLeft(ID) <= 0)
            {
                TooltipUI.Instance().AssignText("No ingredient left...");
                return;
            }

            if (existingIngredient != null)
            {
                ingredient = existingIngredient;
                ingredient.amount++;
            }
            else
            {
                ingredient = new IngredientForm();
                ingredient.amount = 1;
                ingredient.ID = ID;

                allIngredientsInPot.Add(ingredient);
            }

            ConsoleBaksoMain.Instance.RemoveIngredientLeft(ID);
            RefreshAllButtons();

        }

        public void GiveBowlToCustomer(BowlButton bowlButton)
        {

            var customer = ConsoleBaksoMain.Instance.currentCustomerSelected;
            if (customer == null)
            {
                return;
            }

            if (customer.hasBeenServed)
            {
                TooltipUI.Instance().AssignText("Customer has already been served.");
                return;
            }

            customer.FoodServed(bowlButton.bowl);
            allBowlCooked.Remove(bowlButton.bowl);
            RefreshAllBowl();
        }

        public void RefreshTooltip()
        {
            string s = "- ";
            foreach(var ingredient in allIngredientsInPot)
            {
                if (ingredient.amount <= 1)
                {
                    s += $"{ingredient.ID}, ";
                }
                else
                {
                    s += $"x{ingredient.amount} {ingredient.ID}, ";
                }
            }

            if (allIngredientsInPot.Count != 0)
            {
                tooltipText.text = s;
            }
            else
            {
                tooltipText.text = "-drag into the pot";
            }
        }

        public void RefreshAllBowl()
        {
            foreach (var button in allBowlButtons)
            {
                Destroy(button.gameObject);
            }

            allBowlButtons.Clear();

            foreach (var bowl in allBowlCooked)
            {
                var newButton = Instantiate(bowlButtonPrefab, bowlButtonListTransform.transform);
                newButton.transform.localPosition = Vector3.zero;
                newButton.transform.localScale = Vector3.one;
                newButton.bowl = bowl;
                newButton.gameObject.SetActive(true);

                allBowlButtons.Add(newButton);
            }
        }

        private void CreateNewBowl(List<IngredientForm> ingredientForms)
        {
            BowlCooked newBowl = new BowlCooked();

            foreach(var ingredient in ingredientForms)
            {
                newBowl.allIngredients.Add(ingredient);
            }

            allBowlCooked.Add(newBowl);
        }

    }
}