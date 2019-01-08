using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.environment
{
    public abstract class AbstractWeatherSystem : ScriptableObject
    {

        public enum PrecipitationType { Clear, Rain, Snow, Sleet, Hail }
        public enum CloudType { Clear, Light, Heavy, Storm }

        [Header("General Weather System")]
        [Tooltip("Enable automatic updates.")]
        public bool isAuto = true; 

        [Tooltip("Type of current precipitation.")]
        public PrecipitationType precipitationType;
        [Tooltip("Intensity of current precipiitation in mm per hour")]
        [Range(0, 100)]
        public float precipitationIntensity;

        [Tooltip("Type of current cloud cover.")]
        public CloudType cloudType;
        [Tooltip("Intensity of current cloud cover as a % of cover.")]
        [Range(0,100)]
        public float cloudIntensity;

        public float PrecipitationIntensity
        {
            get { return precipitationIntensity; }
            set
            {
                precipitationIntensity = value;
            }
        }

        public bool AutomaticUpdates
        {
            get { return isAuto; }
            set { isAuto = value; }
        }

        /// <summary>
        /// Initialize the Weahter system.
        /// </summary>
        abstract internal void Initialize();

        abstract internal void Update();
    }
}
