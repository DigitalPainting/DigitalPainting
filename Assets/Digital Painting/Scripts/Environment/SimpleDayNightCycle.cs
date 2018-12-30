using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.environment
{
    [CreateAssetMenu(fileName = "Simple Day Night Cycle Config", menuName = "Wizards Code/Simple Day Night Cycle", order = 1)]
    public class SimpleDayNightCycle : AbstractDayNightCycle
    {
        
        override internal void InitializeSun()
        {
            if (sunPrefab == null)
            {
                Debug.LogError("You have not defined a sun in your SimpleDayNightCycle Configuration");
            }
            Sun = Instantiate(sunPrefab).GetComponent<Light>();
        }

        override internal void InitializeSkybox()
        {
            if (skyboxMaterial == null)
            {
                Debug.LogError("You have not defined a skybox material in your SimpleDayNightCycle Configuration");
            }
            RenderSettings.skybox = skyboxMaterial;
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
    }
}