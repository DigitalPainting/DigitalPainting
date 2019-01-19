using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.environment
{
    /// <summary>
    /// The configuration for the scenes Weather implementation.
    /// This is used to define which Weather asset is being used
    /// and to set the basic configuration, such as current weather. Finer
    /// control is managed through your chosen asset.
    /// </summary>
    [AddComponentMenu("Wizards Code/Environment/Weather")]
    public class WeatherManager : MonoBehaviour
    {
        [Header("Weather Manager")]
        [Tooltip("The Weather configuration you want to use. Ensure that the asset required to support this is imported and setup.")]
        public AbstractWeatherSystem configuration;

        [Header("Automated Weather")]
        [Tooltip("Enable automatic updates. Set to false if you want your game to control the weather.")]
        public bool isAuto = true;
        [Tooltip("How frequently The weather should be updated in seconds.")]
        public float WeatherUpdateFrequency = 2;

        public float chanceOfRain = 0.3f;
        public float chanceOfSleet = 0.09f;
        public float chanceOfSnow = 0.05f;
        public float chanceOfHail = 0.01f;

        private float timeToNextUpdate = 0;

        private void Awake()
        {
            if (configuration == null)
            {
                Debug.LogWarning("No configuration provided for the WeatherManager, either remove the component or provide a configuration. For now the component is being disabled.");
                this.enabled = false;
                return;
            }
            configuration.Initialize();
            timeToNextUpdate = 0;
        }

        private void Start()
        {
            configuration.Start();
        }

        /// <summary>
        /// Force an update of the weather immediately. This is useful when the change is created through player interaction or similar.
        /// </summary>
        internal void UpdateNow()
        {
            timeToNextUpdate = 0;
        }

        private void Update()
        {
            timeToNextUpdate -= Time.deltaTime;

            if (timeToNextUpdate > 0)
            {
                return;
            }

            timeToNextUpdate = WeatherUpdateFrequency;

            if (isAuto)
            {
                float value = Random.value;
                if (value <= chanceOfRain)
                {
                    configuration.CurrentProfile.PrecipitationType = WeatherProfile.PrecipitationTypeEnum.Rain;
                    configuration.CurrentProfile.precipitationIntensity = (Random.value * 25) + (Random.value * 25);

                    if (configuration.CurrentProfile.precipitationIntensity <= 10)
                    {
                        configuration.CurrentProfile.cloudType = WeatherProfile.CloudTypeEnum.Light;
                        configuration.CurrentProfile.cloudIntensity = (Random.value * 10) + (Random.value * 10);
                    }
                    else if (configuration.CurrentProfile.precipitationIntensity <= 17)
                    {
                        configuration.CurrentProfile.cloudType = WeatherProfile.CloudTypeEnum.Light;
                        configuration.CurrentProfile.cloudIntensity = 100;
                    }
                    else
                    {
                        configuration.CurrentProfile.cloudType = WeatherProfile.CloudTypeEnum.Heavy;
                        configuration.CurrentProfile.cloudIntensity = 100;
                    }
                }
                else if (value <= chanceOfRain + chanceOfSleet)
                {
                    configuration.CurrentProfile.PrecipitationType = WeatherProfile.PrecipitationTypeEnum.Sleet;
                    configuration.CurrentProfile.precipitationIntensity = (Random.value * 10) + (Random.value * 10);

                    if (configuration.CurrentProfile.precipitationIntensity <= 5)
                    {
                        configuration.CurrentProfile.cloudType = WeatherProfile.CloudTypeEnum.Light;
                        configuration.CurrentProfile.cloudIntensity = 50 + (Random.value * 25) + (Random.value * 25);
                    }
                    else if (configuration.CurrentProfile.precipitationIntensity <= 10)
                    {
                        configuration.CurrentProfile.cloudType = WeatherProfile.CloudTypeEnum.Light;
                        configuration.CurrentProfile.cloudIntensity = 100;
                    }
                    else
                    {
                        configuration.CurrentProfile.cloudType = WeatherProfile.CloudTypeEnum.Heavy;
                        configuration.CurrentProfile.cloudIntensity = 100;
                    }
                }
                else if (value <= chanceOfRain + chanceOfSleet + chanceOfSnow)
                {
                    configuration.CurrentProfile.PrecipitationType = WeatherProfile.PrecipitationTypeEnum.Snow;
                    configuration.CurrentProfile.precipitationIntensity = (Random.value * 5) + (Random.value * 5);

                    if (configuration.CurrentProfile.precipitationIntensity <= 4)
                    {
                        configuration.CurrentProfile.cloudType = WeatherProfile.CloudTypeEnum.Light;
                        configuration.CurrentProfile.cloudIntensity = 50 + (Random.value * 25) + (Random.value * 25);
                    }
                    else if (configuration.CurrentProfile.precipitationIntensity <= 7)
                    {
                        configuration.CurrentProfile.cloudType = WeatherProfile.CloudTypeEnum.Light;
                        configuration.CurrentProfile.cloudIntensity = 100;
                    }
                    else
                    {
                        configuration.CurrentProfile.cloudType = WeatherProfile.CloudTypeEnum.Heavy;
                        configuration.CurrentProfile.cloudIntensity = 100;
                    }
                }
                else if (value <= chanceOfRain + chanceOfSleet + chanceOfSnow + chanceOfHail)
                {
                    configuration.CurrentProfile.PrecipitationType = WeatherProfile.PrecipitationTypeEnum.Hail;
                    configuration.CurrentProfile.precipitationIntensity = (Random.value * 5) + (Random.value * 5);

                    if (configuration.CurrentProfile.precipitationIntensity <= 1)
                    {
                        configuration.CurrentProfile.cloudType = WeatherProfile.CloudTypeEnum.Light;
                        configuration.CurrentProfile.cloudIntensity = 70 + (Random.value * 15) + (Random.value * 15);
                    }
                    else if (configuration.CurrentProfile.precipitationIntensity <= 3)
                    {
                        configuration.CurrentProfile.cloudType = WeatherProfile.CloudTypeEnum.Light;
                        configuration.CurrentProfile.cloudIntensity = 100;
                    }
                    else
                    {
                        configuration.CurrentProfile.cloudType = WeatherProfile.CloudTypeEnum.Heavy;
                        configuration.CurrentProfile.cloudIntensity = 100;
                    }
                }
                else
                {
                    configuration.CurrentProfile.PrecipitationType = WeatherProfile.PrecipitationTypeEnum.Clear;
                    configuration.CurrentProfile.precipitationIntensity = 0;

                    configuration.CurrentProfile.cloudType = WeatherProfile.CloudTypeEnum.Clear;
                    configuration.CurrentProfile.cloudIntensity = 0;
                }
            }

            configuration.Update();
        }
    }
}
