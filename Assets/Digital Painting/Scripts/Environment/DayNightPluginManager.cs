using System;
using UnityEngine;
using wizardscode.plugin;

namespace wizardscode.environment
{
    /// <summary>
    /// The Day Night Cycle Manager is responsible for sharing transferring the Day Night settings to 
    /// the chosen Day Night Cycle implementation.
    /// </summary>
    [AddComponentMenu("Wizards Code/Environment/Day Night Cycle")]
    public class DayNightPluginManager : AbstractPluginManager
    {

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
            get { return Profile.GetTime();  }
        }

        internal AbstractDayNightProfile Profile
        {
            get { return (AbstractDayNightProfile)m_pluginProfile; }
        }

        private void Awake()
        {
            if (Profile == null)
            {
                Debug.LogWarning("No configuration provided for the Day Night Cycle, disabling the `DayNightCycleManager` component. Consider removing, or disabling it permanently.");
                enabled = false;
            }
        }

        private void Start()
        {
            if (Profile == null)
            {
                Debug.LogError("You have not configured the Day Night Cycle.");
            }
            Profile.Initialize();

            dawnStartTime = 0;
            dayStartTime = dawnStartTime + DayNightPluginManager.QUARTER_DAY_AS_SECONDS;
            duskStartTime = dayStartTime + DayNightPluginManager.QUARTER_DAY_AS_SECONDS;
            nightStartTime = duskStartTime + DayNightPluginManager.QUARTER_DAY_AS_SECONDS;

            SetPhase();
        }

        public float DayCycleInMinutes
        {
            get { return Profile.dayCycleInMinutes; }
        }               

        public string ImplementationName
        {
            get { return Profile.name; }
        }

        /// <summary>
        /// Get the current time as a human readable string.
        /// </summary>
        public string CurrentTimeAsLabel
        {
            get {
                TimeSpan t = TimeSpan.FromSeconds(Profile.GetTime());
                string result = string.Format("{0:D2}:{1:D2}",
                                        t.Hours,
                                        t.Minutes);
                return result;
            }
        }
        
        private void Update()
        {
            Profile.Update();
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
