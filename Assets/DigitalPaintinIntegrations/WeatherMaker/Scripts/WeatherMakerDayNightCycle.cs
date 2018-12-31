using DigitalRuby.WeatherMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using wizardscode.environment;

namespace wizardscode.environment.WeatherMaker
{
    [CreateAssetMenu(fileName = "WeatherMakerDayNightCycleConfig", menuName = "Wizards Code/Day Night Cycle/Weather Maker Day Night Cycle Config")]
    public class WeatherMakerDayNightCycle : AbstractDayNightCycle
    {
        [Header("Weather Maker Day Night")]
        [Tooltip("The Weather Maker Profile to use")]
        public WeatherMakerDayNightCycleProfileScript weatherMakerProfile;

        private WeatherMakerDayNightCycleManagerScript manager;

        internal override float GetCurrentTimeInSeconds()
        {
            return manager.TimeOfDay;
        }

        internal override void Initialize(float startTime)
        {
            base.Initialize(startTime);

            manager = WeatherMakerDayNightCycleManagerScript.Instance;
            manager.DayNightProfile = weatherMakerProfile;
        }

        internal override void InitializeCamera()
        {
            Camera.main.farClipPlane = 2500;
            Camera.main.renderingPath = RenderingPath.DeferredLighting;
            Camera.main.allowHDR = true;
            Camera.main.allowMSAA = false;
        }

        internal override void InitializeLighting()
        {
            RenderSettings.sun = Sun;
            RenderSettings.fog = false;
        }

        internal override void InitializeSun()
        {
            if (Sun == null)
            {
                Sun = GameObject.Find("Sun").GetComponent<Light>();
            }
            if (Sun == null)
            {
                Debug.LogError("Cannot find the sun, please set it in the WeatherMakerDayNightCycle configuration");
            }
        }

        internal override void InitializeTiming()
        {
            manager.TimeOfDay = startTime;
            float daySpeed = 1440 / dayCycleInMinutes;
            float nightSpeed = daySpeed;

            weatherMakerProfile.Speed = daySpeed;
            weatherMakerProfile.NightSpeed = daySpeed;

            manager.DayNightProfile = weatherMakerProfile;
            manager.DayNightProfile.UpdateFromProfile(true);
        }

        internal override void Update()
        {
        }
    }
}
