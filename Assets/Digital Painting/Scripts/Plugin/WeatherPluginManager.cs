using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.Plugin;

namespace WizardsCode.Environment
{
    /// <summary>
    /// The Profile for the scenes Weather implementation.
    /// This is used to define which Weather asset is being used
    /// and to set the basic Profile, such as current weather. Finer
    /// control is managed through your chosen asset.
    /// </summary>
    [AddComponentMenu("Wizards Code/Environment/Weather")]
    public class WeatherPluginManager : AbstractPluginManager
    {
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
        
        public AbstractWeatherProfile Profile
        {
            get { return (AbstractWeatherProfile)m_pluginProfile; }
        }

        private void Awake()
        {
            if (Profile == null)
            {
                Debug.LogWarning("No Profile provided for the WeatherManager, either remove the component or provide a Profile. For now the component is being disabled.");
                this.enabled = false;
                return;
            }
            Profile.Initialize();
            timeToNextUpdate = 0;
        }

        private void Start()
        {
            Profile.Start();
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
                    Profile.CurrentProfile.PrecipitationType = WeatherProfile.PrecipitationTypeEnum.Rain;
                    Profile.CurrentProfile.precipitationIntensity = (Random.value * 25) + (Random.value * 25);

                    if (Profile.CurrentProfile.precipitationIntensity <= 10)
                    {
                        Profile.CurrentProfile.cloudType = WeatherProfile.CloudTypeEnum.Light;
                        Profile.CurrentProfile.cloudIntensity = (Random.value * 10) + (Random.value * 10);
                    }
                    else if (Profile.CurrentProfile.precipitationIntensity <= 17)
                    {
                        Profile.CurrentProfile.cloudType = WeatherProfile.CloudTypeEnum.Light;
                        Profile.CurrentProfile.cloudIntensity = 100;
                    }
                    else
                    {
                        Profile.CurrentProfile.cloudType = WeatherProfile.CloudTypeEnum.Heavy;
                        Profile.CurrentProfile.cloudIntensity = 100;
                    }
                }
                else if (value <= chanceOfRain + chanceOfSleet)
                {
                    Profile.CurrentProfile.PrecipitationType = WeatherProfile.PrecipitationTypeEnum.Sleet;
                    Profile.CurrentProfile.precipitationIntensity = (Random.value * 10) + (Random.value * 10);

                    if (Profile.CurrentProfile.precipitationIntensity <= 5)
                    {
                        Profile.CurrentProfile.cloudType = WeatherProfile.CloudTypeEnum.Light;
                        Profile.CurrentProfile.cloudIntensity = 50 + (Random.value * 25) + (Random.value * 25);
                    }
                    else if (Profile.CurrentProfile.precipitationIntensity <= 10)
                    {
                        Profile.CurrentProfile.cloudType = WeatherProfile.CloudTypeEnum.Light;
                        Profile.CurrentProfile.cloudIntensity = 100;
                    }
                    else
                    {
                        Profile.CurrentProfile.cloudType = WeatherProfile.CloudTypeEnum.Heavy;
                        Profile.CurrentProfile.cloudIntensity = 100;
                    }
                }
                else if (value <= chanceOfRain + chanceOfSleet + chanceOfSnow)
                {
                    Profile.CurrentProfile.PrecipitationType = WeatherProfile.PrecipitationTypeEnum.Snow;
                    Profile.CurrentProfile.precipitationIntensity = (Random.value * 5) + (Random.value * 5);

                    if (Profile.CurrentProfile.precipitationIntensity <= 4)
                    {
                        Profile.CurrentProfile.cloudType = WeatherProfile.CloudTypeEnum.Light;
                        Profile.CurrentProfile.cloudIntensity = 50 + (Random.value * 25) + (Random.value * 25);
                    }
                    else if (Profile.CurrentProfile.precipitationIntensity <= 7)
                    {
                        Profile.CurrentProfile.cloudType = WeatherProfile.CloudTypeEnum.Light;
                        Profile.CurrentProfile.cloudIntensity = 100;
                    }
                    else
                    {
                        Profile.CurrentProfile.cloudType = WeatherProfile.CloudTypeEnum.Heavy;
                        Profile.CurrentProfile.cloudIntensity = 100;
                    }
                }
                else if (value <= chanceOfRain + chanceOfSleet + chanceOfSnow + chanceOfHail)
                {
                    Profile.CurrentProfile.PrecipitationType = WeatherProfile.PrecipitationTypeEnum.Hail;
                    Profile.CurrentProfile.precipitationIntensity = (Random.value * 5) + (Random.value * 5);

                    if (Profile.CurrentProfile.precipitationIntensity <= 1)
                    {
                        Profile.CurrentProfile.cloudType = WeatherProfile.CloudTypeEnum.Light;
                        Profile.CurrentProfile.cloudIntensity = 70 + (Random.value * 15) + (Random.value * 15);
                    }
                    else if (Profile.CurrentProfile.precipitationIntensity <= 3)
                    {
                        Profile.CurrentProfile.cloudType = WeatherProfile.CloudTypeEnum.Light;
                        Profile.CurrentProfile.cloudIntensity = 100;
                    }
                    else
                    {
                        Profile.CurrentProfile.cloudType = WeatherProfile.CloudTypeEnum.Heavy;
                        Profile.CurrentProfile.cloudIntensity = 100;
                    }
                }
                else
                {
                    Profile.CurrentProfile.PrecipitationType = WeatherProfile.PrecipitationTypeEnum.Clear;
                    Profile.CurrentProfile.precipitationIntensity = 0;

                    Profile.CurrentProfile.cloudType = WeatherProfile.CloudTypeEnum.Clear;
                    Profile.CurrentProfile.cloudIntensity = 0;
                }
            }

            Profile.Update();
        }
    }
}
