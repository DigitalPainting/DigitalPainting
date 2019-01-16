using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.environment
{
    /// <summary>
    /// A simple day/night controller inspired by wiki.unity3d.com/index.php/DayNightController
    /// </summary>
    [CreateAssetMenu(fileName = "SimpleDayNightCycleConfig", menuName = "Wizards Code/Day Night Cycle/Simple Day Night Cycle")]
    public class SimpleDayNightCycle : AbstractDayNightCycle
    {
        public Color dawnDuskFog = new Color(133.0f / 255.0f, 124.0f / 255.0f, 102.0f / 255.0f);
        public Color dayFog = new Color(180.0f / 255.0f, 208.0f / 255.0f, 209.0f / 255.0f);
        public Color nightFog = new Color(12.0f / 255.0f, 15.0f / 255.0f, 91.0f / 255.0f);

        private float dawnTime;
        private float dayTime;
        private float duskTime;
        private float nightTime;

        protected float currentTimeOfDay;
        private float sunInitialIntensity;
        private Phase currentPhase;

        internal override void InitializeTiming()
        {
            currentTimeOfDay = startTime;

            dawnTime = 0;
            dayTime = dawnTime + QUARTER_DAY_AS_SECONDS;
            duskTime = dayTime + QUARTER_DAY_AS_SECONDS;
            nightTime = duskTime + QUARTER_DAY_AS_SECONDS;

            if (currentTimeOfDay > nightTime)
            {
                currentPhase = Phase.Night;
            }
            else if (currentTimeOfDay > duskTime)
            {
                currentPhase = Phase.Dusk;
            }
            else if (currentTimeOfDay > dayTime)
            {
                currentPhase = Phase.Day;
            }
            else if (currentTimeOfDay > dawnTime && currentTimeOfDay < dayTime)
            {
                currentPhase = Phase.Dawn;
            }
        }

        override internal void InitializeSun()
        {
            if (sunPrefab == null)
            {
                Debug.LogError("You have not defined a sun in your SimpleDayNightCycle Configuration");
            }
            Sun = Instantiate(sunPrefab).GetComponent<Light>();
            sunInitialIntensity = Sun.intensity;
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
            float dayCycleInSeconds = manager.DayCycleInMinutes * 60;
            currentTimeOfDay += Time.deltaTime * (DAY_AS_SECONDS / dayCycleInSeconds);
            if (currentTimeOfDay > DAY_AS_SECONDS)
            {
                currentTimeOfDay -= DAY_AS_SECONDS;
            }

            if (currentTimeOfDay > nightTime && currentPhase == Phase.Dusk)
            {
                currentPhase = Phase.Night;
            }
            else if (currentTimeOfDay > duskTime && currentPhase == Phase.Day)
            {
                currentPhase = Phase.Dusk;
            }
            else if (currentTimeOfDay > dayTime && currentPhase == Phase.Dawn)
            {
                currentPhase = Phase.Day;
            }
            else if (currentTimeOfDay > dawnTime && currentTimeOfDay < dayTime && currentPhase == Phase.Night)
            {
                currentPhase = Phase.Dawn;
            }
        }

        private void UpdateSunPosition()
        {
            Sun.transform.rotation = Quaternion.Euler(new Vector3((currentTimeOfDay - (QUARTER_DAY_AS_SECONDS)) / DAY_AS_SECONDS * 360, 0, 0));
        }

        private void UpdateSunIntensity()
        {
            if (currentPhase == Phase.Dawn)
            {
                float relativeTime = currentTimeOfDay - dawnTime;
                //RenderSettings.ambientLight = Color.Lerp(fullNightAmbientLight, fullDayAmbientLight, relativeTime / (QUARTER_DAY_AS_SECONDS));
                if (Sun != null)
                {
                    Sun.intensity = sunInitialIntensity * (relativeTime / (QUARTER_DAY_AS_SECONDS));
                }
            }
            else if (currentPhase == Phase.Dusk)
            {
                float relativeTime = currentTimeOfDay - duskTime;
                //RenderSettings.ambientLight = Color.Lerp(fullNightAmbientLight, fullDayAmbientLight, relativeTime / (QUARTER_DAY_AS_SECONDS));
                if (Sun != null)
                {
                    Sun.intensity = sunInitialIntensity * (((QUARTER_DAY_AS_SECONDS) - relativeTime) / (QUARTER_DAY_AS_SECONDS));
                }
            }
        }

        private void UpdateFog()
        {
            if (currentPhase == Phase.Dawn)
            {
                float relativeTime = currentTimeOfDay - dawnTime;
                RenderSettings.fogColor = Color.Lerp(dawnDuskFog, dayFog, relativeTime / (QUARTER_DAY_AS_SECONDS));
            }
            else if (currentPhase == Phase.Day)
            {
                float relativeTime = currentTimeOfDay - dayTime;
                RenderSettings.fogColor = Color.Lerp(dayFog, dawnDuskFog, relativeTime / (QUARTER_DAY_AS_SECONDS));
            }
            else if (currentPhase == Phase.Dusk)
            {
                float relativeTime = currentTimeOfDay - duskTime;
                RenderSettings.fogColor = Color.Lerp(dawnDuskFog, nightFog, relativeTime / (QUARTER_DAY_AS_SECONDS));
            }
            else if (currentPhase == Phase.Night)
            {
                float relativeTime = currentTimeOfDay - nightTime;
                RenderSettings.fogColor = Color.Lerp(nightFog, dawnDuskFog, relativeTime / (QUARTER_DAY_AS_SECONDS));
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

        public enum Phase { Night, Dawn, Day, Dusk }
    }
}