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
                        precipitationType = PrecipitationType.Rain;
                        precipitationIntensity = (Random.value * 25) + (Random.value * 25);

                        if (precipitationIntensity <= 10)
                        {
                            cloudType = CloudType.Light;
                            cloudIntensity = (Random.value * 10) + (Random.value * 10);
                        }
                        else if (precipitationIntensity <= 17)
                        {
                            cloudType = CloudType.Light;
                            cloudIntensity = 100;
                        }
                        else
                        {
                            cloudType = CloudType.Heavy;
                            cloudIntensity = 100;
                        }
                    }
                    else if (value <= chanceOfRain + chanceOfSleet)
                    {
                        precipitationType = PrecipitationType.Sleet;
                        precipitationIntensity = (Random.value * 10) + (Random.value * 10);

                        if (precipitationIntensity <= 5)
                        {
                            cloudType = CloudType.Light;
                            cloudIntensity = 50 + (Random.value * 25) + (Random.value * 25);
                        }
                        else if (precipitationIntensity <= 10)
                        {
                            cloudType = CloudType.Light;
                            cloudIntensity = 100;
                        }
                        else
                        {
                            cloudType = CloudType.Heavy;
                            cloudIntensity = 100;
                        }
                    }
                    else if (value <= chanceOfRain + chanceOfSleet + chanceOfSnow)
                    {
                        precipitationType = PrecipitationType.Snow;
                        precipitationIntensity = (Random.value * 5) + (Random.value * 5);

                        if (precipitationIntensity <= 4)
                        {
                            cloudType = CloudType.Light;
                            cloudIntensity = 50 + (Random.value * 25) + (Random.value * 25);
                        }
                        else if (precipitationIntensity <= 7)
                        {
                            cloudType = CloudType.Light;
                            cloudIntensity = 100;
                        }
                        else
                        {
                            cloudType = CloudType.Heavy;
                            cloudIntensity = 100;
                        }
                    }
                    else if (value <= chanceOfRain + chanceOfSleet + chanceOfSnow + chanceOfHail)
                    {
                        precipitationType = PrecipitationType.Hail;
                        precipitationIntensity = (Random.value * 5) + (Random.value * 5);

                        if (precipitationIntensity <= 1)
                        {
                            cloudType = CloudType.Light;
                            cloudIntensity = 70 + (Random.value * 15) + (Random.value * 15);
                        }
                        else if (precipitationIntensity <= 3)
                        {
                            cloudType = CloudType.Light;
                            cloudIntensity = 100;
                        }
                        else
                        {
                            cloudType = CloudType.Heavy;
                            cloudIntensity = 100;
                        }
                    }
                    else
                    {
                        precipitationType = PrecipitationType.Clear;
                        precipitationIntensity = 0;

                        cloudType = CloudType.Clear;
                        cloudIntensity = 0;
                    }
                }

                string summary = precipitationType + ". Time since start: " + Time.realtimeSinceStartup;
                string report;
                if (precipitationType != PrecipitationType.Clear)
                {
                    report = precipitationType + "(" + precipitationIntensity + " mm/h)";
                } else
                {
                    report = "No rain";
                }
                if (cloudType != CloudType.Clear)
                {
                    report += " with " + cloudIntensity + "% " + cloudType + " clouds.";
                } else
                {
                    report += " and no clouds.";
                }
                Debug.Log("Current weather report: " + summary + "\n" + report);
            }
        }
    }
}