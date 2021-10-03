using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaksoGame
{
    public class DayNightCycle : MonoBehaviour
    {

        public Animator dayAnimator;
        public float timeScale = 1;
        public float initialHour = 6;

        private float time = 0;
        private float timeline = 0;

        private void Start()
        {
            SetCurrentTime(initialHour);
        }

        [ContextMenu("SetEvening")]
        public void SetEvening()
        {
            SetCurrentTime(16);
        }

        public void SetCurrentTime(float hour)
        {
            time = hour / 24f;
        }

        public float GetCurrentTime()
        {
            return timeline;
        }

        private void Update()
        {
            dayAnimator.SetFloat("DayNight", time);

            if (ConsoleBaksoMain.Instance.isDayStarted == false)
            {
                return;
            }

            time += timeScale * 0.001f * Time.deltaTime;

            if (time >= 1)
            {
                time = 0;
            }

            timeline = time * 24;
        }

    }
}