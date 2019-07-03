using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.environment;

namespace WizardsCode.Agent
{
    /// <summary>
    /// Turn the lights on around sunset and off around sunrise. If there is no DayNightCycleManager
    /// in the scene then this script will disable itself.
    /// </summary>
    public class LightsOnOff : MonoBehaviour
    {
        [Tooltip("Do we want to fade lights in/out (set to true) or switch them on/off (set to false)")]
        public bool isFade = true;
        [Tooltip("The lights to control")]
        public Light[] lights;
        
        DayNightPluginManager dayNightManager;
        private float[] maxIntensity;

        void Start()
        {
            dayNightManager = FindObjectOfType<DayNightPluginManager>();
            if (dayNightManager == null)
            {
                Debug.LogWarning("There is an active LightsOnOff script in the scene, but no DayNightCycleManager. Disabling the LightsOnOff script on " + gameObject.name);
                enabled = false;
            }

            maxIntensity = new float[lights.Length];
            for (int i = 0; i < lights.Length; i++)
            {
                maxIntensity[i] = lights[i].intensity;
            }
        }

        void Update()
        {
            if (isFade)
            {
                fadeLights();
            }
            else
            {
                switchLights();
            }

        }

        private void switchLights()
        {
            if (dayNightManager.CurrentPhase == DayNightPluginManager.Phase.Dusk || dayNightManager.CurrentPhase == DayNightPluginManager.Phase.Dawn)
            {
                for (int i = 0; i < lights.Length; i++)
                {
                    lights[i].intensity = maxIntensity[i] * 0.5f;
                }
            }
            else if (dayNightManager.CurrentPhase == DayNightPluginManager.Phase.Night)
            {
                for (int i = 0; i < lights.Length; i++)
                {
                    lights[i].intensity = maxIntensity[i];
                }
            }
            else
            {
                for (int i = 0; i < lights.Length; i++)
                {
                    lights[i].intensity = 0f;
                }
            }
        }

        private void fadeLights()
        {
            for (int i = 0; i < lights.Length; i++)
            {
                if (dayNightManager.CurrentPhase == DayNightPluginManager.Phase.Dawn)
                {
                    float relativeTime = dayNightManager.CurrentTime - dayNightManager.dawnStartTime;
                    lights[i].intensity = maxIntensity[i] * ((DayNightPluginManager.QUARTER_DAY_AS_SECONDS - relativeTime) / DayNightPluginManager.QUARTER_DAY_AS_SECONDS);
                }
                else if (dayNightManager.CurrentPhase == DayNightPluginManager.Phase.Dusk)
                {
                    float relativeTime = dayNightManager.CurrentTime - dayNightManager.duskStartTime;
                    lights[i].intensity = maxIntensity[i] * (relativeTime / DayNightPluginManager.QUARTER_DAY_AS_SECONDS);
                }
                else if (dayNightManager.CurrentPhase == DayNightPluginManager.Phase.Dusk)
                {
                    lights[i].intensity = maxIntensity[i];
                }
                else if (dayNightManager.CurrentPhase == DayNightPluginManager.Phase.Day)
                {
                    lights[i].intensity = 0;
                }
            }
        }
    }
}
