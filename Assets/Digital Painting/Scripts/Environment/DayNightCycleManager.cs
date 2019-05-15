using System;
using UnityEngine;

namespace wizardscode.environment
{
    /// <summary>
    /// The configuration for the scenes Day Night Cycle implementation.
    /// This is used to define which Day Night Cycle asset is being used
    /// and to set the basic configuration, such as time of day. Finer
    /// control is managed through your chosen asset.
    /// </summary>
    [AddComponentMenu("Wizards Code/Environment/Day Night Cycle")]
    public class DayNightCycleManager : MonoBehaviour
    {
        [Header("Timing")]
        [Tooltip("Start time in seconds. 0 and 86400 is midnight.")]
        [Range(0, 86400)]
        public float startTime = 5 * 60 * 60; // (5 AM)
        [Tooltip("The speed at which a game day passes in real-time.")]
        public float dayCycleInMinutes = 1;

        [Header("Environment settings")]
        [Tooltip("Skybox materials to use.")]
        public Material skybox;
        [Tooltip("A prefab containing the directional light that acts as the sun. If blank a light with the name `Sun` will be used.")]
        public Light sunPrefab;

        [Tooltip("The Day Night Cycle configuration you want to use. Ensure that the asset required to support this is imported and setup.")]
        public AbstractDayNightProfile configuration;

        public const float SECOND = 1;
        public const float MINUTE_AS_SECONDS = 60 * SECOND;
        public const float HOUR_AS_SECONDS = 60 * MINUTE_AS_SECONDS;
        public const float DAY_AS_SECONDS = 24 * HOUR_AS_SECONDS;
        public const float QUARTER_DAY_AS_SECONDS = DAY_AS_SECONDS / 4;
        public const float DEGREES_PER_SECOND = 360 / DAY_AS_SECONDS;

        internal float dawnStartTime;
        internal float dayStartTime;
        internal float duskStartTime;
        internal float nightStartTime;
        public enum Phase { Night, Dawn, Day, Dusk }
        private Phase _currentPhase;
        public Phase CurrentPhase
        {
            get { return _currentPhase; }
            set { _currentPhase = value; }
        }

        public float CurrentTime
        {
            get { return configuration.GetTime();  }
        }

        private void Awake()
        {
            if (configuration == null)
            {
                Debug.LogWarning("No configuration provided for the Day Night Cycle, disabling the `DayNightCycleManager` component. Consider removing, or disabling it permanently.");
                enabled = false;
            }
        }

        private void Start()
        {
            if (configuration == null)
            {
                Debug.LogError("You have not configured the Day Night Cycle.");
            }
            configuration.Initialize(startTime);

            dawnStartTime = 0;
            dayStartTime = dawnStartTime + DayNightCycleManager.QUARTER_DAY_AS_SECONDS;
            duskStartTime = dayStartTime + DayNightCycleManager.QUARTER_DAY_AS_SECONDS;
            nightStartTime = duskStartTime + DayNightCycleManager.QUARTER_DAY_AS_SECONDS;

            SetPhase();
        }

        public float DayCycleInMinutes
        {
            get { return dayCycleInMinutes; }
        }               

        public string ImplementationName
        {
            get { return configuration.name; }
        }

        /// <summary>
        /// Get the current time as a human readable string.
        /// </summary>
        public string CurrentTimeAsLabel
        {
            get {
                TimeSpan t = TimeSpan.FromSeconds(configuration.GetTime());
                string result = string.Format("{0:D2}:{1:D2}",
                                        t.Hours,
                                        t.Minutes);
                return result;
            }
        }
        
        private void Update()
        {
            configuration.Update();
            SetPhase();
        }

        private void SetPhase()
        {
            if (CurrentTime >= nightStartTime)
            {
                CurrentPhase = Phase.Night;
            }
            else if (CurrentTime >= duskStartTime && CurrentTime < nightStartTime)
            {
                CurrentPhase = Phase.Dusk;
            }
            else if (CurrentTime >= dayStartTime && CurrentTime < duskStartTime)
            {
                CurrentPhase = Phase.Day;
            }
            else if (CurrentTime >= dawnStartTime && CurrentTime < dayStartTime)
            {
                CurrentPhase = Phase.Dawn;
            }
        }

        public float GameSecondsToRealSeconds(float gameSeconds)
        {
            float realSecondsPerGameSecond = DAY_AS_SECONDS / (DayCycleInMinutes * MINUTE_AS_SECONDS);
            return gameSeconds / realSecondsPerGameSecond;
        }
    }
}
