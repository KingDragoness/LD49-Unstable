using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaksoGame
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Ingredient", order = 1)]
    public class Ingredient : ScriptableObject
    {
        public string ingredientName = "Water";
        public Sprite sprite;
        public bool isEssential = false;
        public int priceBuy = 10000;

        //t: 0 - 1
        //value: 0 - 10
        public AnimationCurve randomDistribution;
    }
}