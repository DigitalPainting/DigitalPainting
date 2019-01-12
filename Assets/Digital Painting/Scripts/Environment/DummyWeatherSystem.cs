using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.environment
{
    [CreateAssetMenu(fileName = "DummyWeatherSystem", menuName = "Wizards Code/Weather/Dummy Weather System (for testing only)")]
    public class DummyWeatherSystem : AbstractWeatherSystem
    {
        [Tooltip("How frequently The weather should be updated in seconds.")]
        public float WeatherUpdateFrequency = 2 ;

        public float chanceOfRain = 0.3f;
        public float chanceOfSleet = 0.09f;
        public float chanceOfSnow = 0.05f;
        public float chanceOfHail = 0.01f;

        private float timeToNextUpdate = 0;
        
        internal override void Initialize()
        {
            // Nothing to do here
        }

        internal override void Start()
        {
            // Nothing to do here
        }

        override internal void Update()
        {
            timeToNextUpdate -= Time.deltaTime;
            if (timeToNextUpdate < 0)
            {
                timeToNextUpdate = WeatherUpdateFrequency;
                if (AutomaticUpdates)
                {
                    float value = Random.value;
                    if (value <= chanceOfRain)
                    {
                        currentProfile.precipitationType = WeatherProfile.PrecipitationTypeEnum.Rain;
                        currentProfile.precipitationIntensity = (Random.value * 25) + (Random.value * 25);

                        if (currentProfile.precipitationIntensity <= 10)
                        {
                            currentProfile.cloudType = WeatherProfile.CloudTypeEnum.Light;
                            currentProfile.cloudIntensity = (Random.value * 10) + (Random.value * 10);
                        }
                        else if (currentProfile.precipitationIntensity <= 17)
                        {
                            currentProfile.cloudType = WeatherProfile.CloudTypeEnum.Light;
                            currentProfile.cloudIntensity = 100;
                        }
                        else
                        {
                            currentProfile.cloudType = WeatherProfile.CloudTypeEnum.Heavy;
                            currentProfile.cloudIntensity = 100;
                        }
                    }
                    else if (value <= chanceOfRain + chanceOfSleet)
                    {
                        currentProfile.precipitationType = WeatherProfile.PrecipitationTypeEnum.Sleet;
                        currentProfile.precipitationIntensity = (Random.value * 10) + (Random.value * 10);

                        if (currentProfile.precipitationIntensity <= 5)
                        {
                            currentProfile.cloudType = WeatherProfile.CloudTypeEnum.Light;
                            currentProfile.cloudIntensity = 50 + (Random.value * 25) + (Random.value * 25);
                        }
                        else if (currentProfile.precipitationIntensity <= 10)
                        {
                            currentProfile.cloudType = WeatherProfile.CloudTypeEnum.Light;
                            currentProfile.cloudIntensity = 100;
                        }
                        else
                        {
                            currentProfile.cloudType = WeatherProfile.CloudTypeEnum.Heavy;
                            currentProfile.cloudIntensity = 100;
                        }
                    }
                    else if (value <= chanceOfRain + chanceOfSleet + chanceOfSnow)
                    {
                        currentProfile.precipitationType = WeatherProfile.PrecipitationTypeEnum.Snow;
                        currentProfile.precipitationIntensity = (Random.value * 5) + (Random.value * 5);

                        if (currentProfile.precipitationIntensity <= 4)
                        {
                            currentProfile.cloudType = WeatherProfile.CloudTypeEnum.Light;
                            currentProfile.cloudIntensity = 50 + (Random.value * 25) + (Random.value * 25);
                        }
                        else if (currentProfile.precipitationIntensity <= 7)
                        {
                            currentProfile.cloudType = WeatherProfile.CloudTypeEnum.Light;
                            currentProfile.cloudIntensity = 100;
                        }
                        else
                        {
                            currentProfile.cloudType = WeatherProfile.CloudTypeEnum.Heavy;
                            currentProfile.cloudIntensity = 100;
                        }
                    }
                    else if (value <= chanceOfRain + chanceOfSleet + chanceOfSnow + chanceOfHail)
                    {
                        currentProfile.precipitationType = WeatherProfile.PrecipitationTypeEnum.Hail;
                        currentProfile.precipitationIntensity = (Random.value * 5) + (Random.value * 5);

                        if (currentProfile.precipitationIntensity <= 1)
                        {
                            currentProfile.cloudType = WeatherProfile.CloudTypeEnum.Light;
                            currentProfile.cloudIntensity = 70 + (Random.value * 15) + (Random.value * 15);
                        }
                        else if (currentProfile.precipitationIntensity <= 3)
                        {
                            currentProfile.cloudType = WeatherProfile.CloudTypeEnum.Light;
                            currentProfile.cloudIntensity = 100;
                        }
                        else
                        {
                            currentProfile.cloudType = WeatherProfile.CloudTypeEnum.Heavy;
                            currentProfile.cloudIntensity = 100;
                        }
                    }
                    else
                    {
                        currentProfile.precipitationType = WeatherProfile.PrecipitationTypeEnum.Clear;
                        currentProfile.precipitationIntensity = 0;

                        currentProfile.cloudType = WeatherProfile.CloudTypeEnum.Clear;
                        currentProfile.cloudIntensity = 0;
                    }
                }

                string summary = currentProfile.precipitationType + ". Time since start: " + Time.realtimeSinceStartup;
                string report;
                if (currentProfile.precipitationType != WeatherProfile.PrecipitationTypeEnum.Clear)
                {
                    report = currentProfile.precipitationType + "(" + currentProfile.precipitationIntensity + " mm/h)";
                } else
                {
                    report = "No rain";
                }
                if (currentProfile.cloudType != WeatherProfile.CloudTypeEnum.Clear)
                {
                    report += " with " + currentProfile.cloudIntensity + "% " + currentProfile.cloudType + " clouds.";
                } else
                {
                    report += " and no clouds.";
                }
                Debug.Log("Current weather report: " + summary + "\n" + report);
            }
        }
    }
}