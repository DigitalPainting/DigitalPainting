using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.environment
{
    /// <summary>
    /// A simple day/night controller inspired by wiki.unity3d.com/index.php/DayNightController
    /// </summary>
    [CreateAssetMenu(fileName = "SimpleDayNightProfile", menuName = "Wizards Code/Day Night Cycle/Simple Day Night Profile")]
    public class SimpleDayNightProfile : AbstractDayNightProfile
    {
        [Header("Simple Day Night Config")]
        public Color dawnDuskFog = new Color(133.0f / 255.0f, 124.0f / 255.0f, 102.0f / 255.0f);
        public Color dayFog = new Color(180.0f / 255.0f, 208.0f / 255.0f, 209.0f / 255.0f);
        public Color nightFog = new Color(12.0f / 255.0f, 15.0f / 255.0f, 91.0f / 255.0f);

        protected float currentTimeOfDay;
        private float sunInitialIntensity;
        
        internal override void InitializeTiming()
        {
            currentTimeOfDay = startTime;
        }

        override internal void InitializeSun()
        {
            sunInitialIntensity = RenderSettings.sun.intensity;
        }

        override internal void Update()
        {
            UpdateTime();
            UpdateSunPosition();
            UpdateSunIntensity();
            UpdateFog();
        }

        public void UpdateTime()
        {
            float dayCycleInSeconds = dayCycleInMinutes * 60;
            currentTimeOfDay += Time.deltaTime * (DayNightPluginManager.DAY_AS_SECONDS / dayCycleInSeconds);
            if (currentTimeOfDay > DayNightPluginManager.DAY_AS_SECONDS)
            {
                currentTimeOfDay -= DayNightPluginManager.DAY_AS_SECONDS;
            }
        }

        private void UpdateSunPosition()
        {
            Sun.transform.rotation = Quaternion.Euler(new Vector3((currentTimeOfDay - (DayNightPluginManager.QUARTER_DAY_AS_SECONDS)) / DayNightPluginManager.DAY_AS_SECONDS * 360, 0, 0));
        }

        private void UpdateSunIntensity()
        {
            if (manager.CurrentPhase == DayNightPluginManager.Phase.Dawn)
            {
                float relativeTime = currentTimeOfDay - manager.dawnStartTime;
                //RenderSettings.ambientLight = Color.Lerp(fullNightAmbientLight, fullDayAmbientLight, relativeTime / (QUARTER_DAY_AS_SECONDS));
                if (Sun != null)
                {
                    Sun.intensity = sunInitialIntensity * (relativeTime / (DayNightPluginManager.QUARTER_DAY_AS_SECONDS));
                }
            }
            else if (manager.CurrentPhase == DayNightPluginManager.Phase.Dusk)
            {
                float relativeTime = currentTimeOfDay - manager.duskStartTime;
                //RenderSettings.ambientLight = Color.Lerp(fullNightAmbientLight, fullDayAmbientLight, relativeTime / (QUARTER_DAY_AS_SECONDS));
                if (Sun != null)
                {
                    Sun.intensity = sunInitialIntensity * (((DayNightPluginManager.QUARTER_DAY_AS_SECONDS) - relativeTime) / (DayNightPluginManager.QUARTER_DAY_AS_SECONDS));
                }
            }
        }

        private void UpdateFog()
        {
            if (manager.CurrentPhase == DayNightPluginManager.Phase.Dawn)
            {
                float relativeTime = currentTimeOfDay - manager.dawnStartTime;
                RenderSettings.fogColor = Color.Lerp(dawnDuskFog, dayFog, relativeTime / (DayNightPluginManager.QUARTER_DAY_AS_SECONDS));
            }
            else if (manager.CurrentPhase == DayNightPluginManager.Phase.Day)
            {
                float relativeTime = currentTimeOfDay - manager.dayStartTime;
                RenderSettings.fogColor = Color.Lerp(dayFog, dawnDuskFog, relativeTime / (DayNightPluginManager.QUARTER_DAY_AS_SECONDS));
            }
            else if (manager.CurrentPhase == DayNightPluginManager.Phase.Dusk)
            {
                float relativeTime = currentTimeOfDay - manager.duskStartTime;
                RenderSettings.fogColor = Color.Lerp(dawnDuskFog, nightFog, relativeTime / (DayNightPluginManager.QUARTER_DAY_AS_SECONDS));
            }
            else if (manager.CurrentPhase == DayNightPluginManager.Phase.Night)
            {
                float relativeTime = currentTimeOfDay - manager.nightStartTime;
                RenderSettings.fogColor = Color.Lerp(nightFog, dawnDuskFog, relativeTime / (DayNightPluginManager.QUARTER_DAY_AS_SECONDS));
            }
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