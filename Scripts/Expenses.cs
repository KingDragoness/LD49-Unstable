using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaksoGame
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Expenses", order = 1)]
    public class Expenses : ScriptableObject
    {
        public string expenseName = "Electricity";
        public int expenseTotal = 50000;

        public float chanceToGet = 1f;

    }
}