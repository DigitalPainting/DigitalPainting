using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.environment
{
    [CreateAssetMenu(fileName = "SimpleDayNightCycleConfig", menuName = "Wizards Code/Day Night Cycle/Simple Day Night Cycle")]
    public class SimpleDayNightCycle : AbstractDayNightCycle
    {
        protected float currentTimeOfDay;

        internal override void InitializeTiming()
        {
            currentTimeOfDay = startTime;
        }

        override internal void InitializeSun()
        {
            if (sunPrefab == null)
            {
                Debug.LogError("You have not defined a sun in your SimpleDayNightCycle Configuration");
            }
            Sun = Instantiate(sunPrefab).GetComponent<Light>();
        }

        override internal void Update()
        {
            UpdateTime();
            UpdateSunPosition();
        }

        public void UpdateTime()
        {
            float dayCycleInSeconds = dayCycleInMinutes * 60;
            currentTimeOfDay += Time.deltaTime * (DAY_AS_SECONDS / dayCycleInSeconds);
            if (currentTimeOfDay > DAY_AS_SECONDS)
            {
                currentTimeOfDay -= DAY_AS_SECONDS;
            }
        }

        private void UpdateSunPosition()
        {
            Sun.transform.rotation = Quaternion.Euler(new Vector3((currentTimeOfDay - (DAY_AS_SECONDS / 4)) / DAY_AS_SECONDS * 360, 0, 0));
        }

        internal override float GetTime()
        {
            return currentTimeOfDay;
        }

        internal override void InitializeCamera()
        {
            // Nothing special to do here
        }

        internal override void InitializeLighting()
        {
            // Nothing special to do here
        }

        internal override void SetTime(float timeInSeconds)
        {
            currentTimeOfDay = timeInSeconds;
        }
    }
}