using System;
using UnityEngine;
using WizardsCode.Editor;
using WizardsCode.Plugin;
using WizardsCode.Validation;

namespace WizardsCode.Environment
{
    public abstract class AbstractWeatherProfile : AbstractPluginProfile
    {
        [Header("Environment settings")]
        [Expandable(isRequired: true, isRequiredMessage: "Must provide a suggested skybox setting.")]
        public SkyBoxSettingsSO Skybox;
        
        private WeatherProfile _currentProfile;
        public virtual WeatherProfile CurrentProfile
        {
            get { return _currentProfile; }
            set
            {
                if (_currentProfile != value)
                {
                    _currentProfile = value;
                }
            }
        }

        /// <summary>
        /// Initialize the Weather system. This is called by the WeatherManager Awake method.
        /// </summary>
        abstract internal void Initialize();

        /// <summary>
        /// Start the Weather System. This is called by the WeatherManager Start method.
        /// </summary>
        abstract internal void Start();

        /// <summary>
        /// Update the Weather. This is called by the WeatherManager Update method.
        /// </summary>
        abstract internal void Update();
    }

    [Serializable]
    public class WeatherProfile
    {
        public enum PrecipitationTypeEnum { Clear, Rain, Snow, Sleet, Hail }
        public enum CloudTypeEnum { Clear, Light, Heavy, Storm }

        [Tooltip("Intensity of current precipitation in mm per hour")]
        public float precipitationIntensity;

        [Tooltip("Type of current cloud cover.")]
        public CloudTypeEnum cloudType;
        [Tooltip("Intensity of current cloud as a % of cover.")]
        [Range(0, 100)]
        public float cloudIntensity;

        internal bool isDirty = false;

        private PrecipitationTypeEnum _precipitationType;
        public PrecipitationTypeEnum PrecipitationType
        {
            get { return _precipitationType; }
            set
            {
                if (_precipitationType != value)
                {
                    _precipitationType = value;
                    if (value == PrecipitationTypeEnum.Clear)
                    {
                        PrecipitationIntensity = 0;
                    }
                    isDirty = true;
                }
            }
        }

        public CloudTypeEnum CloudType
        {
            get { return cloudType; }
            set
            {
                cloudType = value;
                if (value == CloudTypeEnum.Clear)
                {
                    cloudIntensity = 0;
                }
            }
        }

        public WeatherProfile(PrecipitationTypeEnum precipType, CloudTypeEnum cloudType)
        {
            PrecipitationType = precipType;
            CloudType = cloudType;
        }

        public float PrecipitationIntensity
        {
            get { return precipitationIntensity; }
            set
            {
                precipitationIntensity = value;
            }
        }

        public override string ToString()
        {
            string report;
            if (_precipitationType != WeatherProfile.PrecipitationTypeEnum.Clear)
            {
                report = _precipitationType + "(" + precipitationIntensity + " mm/h)";
            }
            else
            {
                report = "No rain";
            }
            if (cloudType != WeatherProfile.CloudTypeEnum.Clear)
            {
                report += " with " + cloudIntensity + "% " + cloudType + " clouds.";
            }
            else
            {
                report += " and no clouds.";
            }

            return report;
        }
    }
}
