using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace BaksoGame
{
    public class ConsoleBaksoMain : MonoBehaviour
    {

        [Header("Base")]
        public List<Ingredient> allIngredients = new List<Ingredient>();
        public List<Expenses> allExpenses = new List<Expenses>();
        public OrderanUI orderanUI;
        public DayNightCycle dayNightCycle;
        public CinemachineVirtualCamera virtualCam;
        public GameObject spawnArea;
        public LayerMask detectRaycastLayer;

        [Space]
        [Header("Daily Report")]
        public int todayMoneyEarned = 0;
        public List<string> todayExpenses = new List<string>();

        [Space]
        public List<IngredientForm> ingredientStorage = new List<IngredientForm>();
        public int totalMoney = 1000;
        public int day = 1;
        public float hitpointGerobak = 100;
        public bool isDayStarted = false;
        public Customer currentCustomerSelected;

        public static ConsoleBaksoMain Instance;

        [Header("Grid Size")]
        public Vector2 gridSize = new Vector2(10, 10);

        private bool isServingMode = false;
        private bool isDead = false;

        public bool IsServingMode { get => isServingMode; }
        public bool IsDead { get => isDead; set => isDead = value; }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x*2, 1f, gridSize.y*2));
        }

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            foreach (var ingredient in allIngredients)
            {
                int amount = 50;

                IngredientForm form = new IngredientForm();
                form.ID = ingredient.ingredientName;
                form.amount = amount;

                ingredientStorage.Add(form);
            }
        }

        public int GetAmountIngredientLeft(string ID)
        {
            return ingredientStorage.Find(x => x.ID == ID).amount;
        }

        public void AddIngredientLeft(string ID, int amount)
        {
            var ingredient = ingredientStorage.Find(x => x.ID == ID);

            ingredient.amount += amount;
        }


        public void RemoveIngredientLeft(string ID)
        {
            var ingredient = ingredientStorage.Find(x => x.ID == ID);

            if (ingredient.amount <= 0)
            {
                return;
            }

            ingredient.amount--;
        }

        public Ingredient GetIngredient(string ID)
        {
            return allIngredients.Find(x => x.ingredientName == ID);
        }

        [ContextMenu("StartNewLevel")]
        public void StartNewLevel()
        {
            dayNightCycle.SetCurrentTime(dayNightCycle.initialHour);
            GerobakController.instance.transform.position = spawnArea.transform.position;
            GerobakController.instance.transform.eulerAngles = spawnArea.transform.eulerAngles;

            day++;
            hitpointGerobak = 100;
            todayExpenses.Clear();
            GenerateRandomExpenses();
            todayMoneyEarned = 0;
            isDayStarted = true;
            BaksoMainUI.instance.tingtingSFX.Play();
            PremanSpawner.Instance.ClearAll();
            NPCSpawner.instance.SpawnNewLevel();
            Cursor.lockState = CursorLockMode.Locked;
            isServingMode = false;
            RefreshMode();
        }

        private void GenerateRandomExpenses()
        {
            foreach(var expenseType in allExpenses)
            {
                float random = Random.Range(0f, 1f);

                if (expenseType.chanceToGet == 1)
                {

                }
                else if (random > expenseType.chanceToGet)
                {
                    continue;
                }

                todayExpenses.Add(expenseType.expenseName);
            }
        }

        public Expenses GetExpense(string ID)
        {
            return allExpenses.Find(x => x.expenseName == ID);
        }

        public int TotalExpenses()
        {
            int result = 0;
            foreach (var expense in todayExpenses)
            {
                result += GetExpense(expense).expenseTotal;

            }

            return result;
        }

        public void EndDay()
        {
            foreach(var expense in todayExpenses)
            {
                totalMoney -= GetExpense(expense).expenseTotal;

            }
            isDayStarted = false;
            Cursor.lockState = CursorLockMode.None;
            BaksoMainUI.instance.OpenReportSession();
            dayNightCycle.SetCurrentTime(20);
        }

        void Update()
        {
            if (isDead | isDayStarted == false)
            {
                return;
            }

            if (Input.GetKeyUp(KeyCode.T))
            {
                isServingMode = !isServingMode;
                RefreshMode();
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (currentCustomerSelected != null)
                    HighlightOrder();

            }

        }
        private void FixedUpdate()
        {
            if (isDead && isDayStarted == false)
            {
                return;
            }
            RaycastCustomers();
        }
        public void DamagePlayer(float damageAmount = 10)
        {
            hitpointGerobak -= damageAmount;
            if (hitpointGerobak < 0)
            {
                Die();
                hitpointGerobak = 0;
            }
        }

        public void Die()
        {
            if (isDead)
            {
                return;
            }

            BaksoMainUI.instance.failSFX.Play();
            BaksoMainUI.instance.gameoverScreen.SetActive(true);
            isDead = true;
        }



        private void RaycastCustomers()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool noCustomerDetected = true;

            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(ray, out hit, 100, detectRaycastLayer))
            {
                var customer = hit.collider.GetComponent<Customer>();

                if (customer != null)
                {
                    currentCustomerSelected = customer;
                    currentCustomerSelected.SelectHighlight();
                    noCustomerDetected = false;

                }
            }

            if (noCustomerDetected)
            {
                currentCustomerSelected = null;
            }
        }

        private void HighlightOrder()
        {
            string s = "- ";
            foreach (var ingredient in currentCustomerSelected.needsIngredients)
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

            OrderPaperUI.Instance().gameObject.SetActive(true);
            OrderPaperUI.Instance().AssignText(s);

        }

        CinemachineOrbitalTransposer cinemachineOrbital = null;
        CinemachineGroupComposer groupComposer = null;

        private void RefreshMode()
        {
            GerobakController.instance.RefreshGerobak();

            var cOrbital1 = virtualCam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
            var cComposer1 = virtualCam.GetCinemachineComponent<CinemachineGroupComposer>();

            if (cOrbital1 != null)
            {
                cinemachineOrbital = cOrbital1;
            }
            if (cComposer1 != null)
            {
                groupComposer = cComposer1;
            }

            if (isServingMode)
            {
                Cursor.lockState = CursorLockMode.None;
                orderanUI.gameObject.SetActive(true);
                virtualCam.gameObject.SetActive(false);

            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                orderanUI.gameObject.SetActive(false);
                virtualCam.gameObject.SetActive(true);


            }
        }
    }
}