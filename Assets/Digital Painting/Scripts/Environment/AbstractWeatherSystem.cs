using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.environment
{
    public abstract class AbstractWeatherSystem : ScriptableObject
    {
        public WeatherProfile currentProfile;

        [Header("General Weather System")]
        [Tooltip("Enable automatic updates.")]
        public bool isAuto = true; 
        
        public bool AutomaticUpdates
        {
            get { return isAuto; }
            set { isAuto = value; }
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

        [Tooltip("Type of current precipitation.")]
        public PrecipitationTypeEnum precipitationType;
        [Tooltip("Intensity of current precipitation in mm per hour")]
        public float precipitationIntensity;

        [Tooltip("Type of current cloud cover.")]
        public CloudTypeEnum cloudType;
        [Tooltip("Intensity of current cloud as a % of cover.")]
        [Range(0, 100)]
        public float cloudIntensity;

        public PrecipitationTypeEnum PrecipitationType
        {
            get { return precipitationType; }
            set
            {
                precipitationType = value;
                if (value == PrecipitationTypeEnum.Clear)
                {
                    PrecipitationIntensity = 0;
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
    }
}
