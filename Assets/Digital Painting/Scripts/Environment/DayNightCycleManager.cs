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
        [Tooltip("Start time in seconds. 0 and 86400 is midnight.")]
        [Range(0, 86400)]
        public float startTime = 5 * 60 * 60; // (5 AM)

        [Tooltip("The Day Night Cycle configuration you want to use. Ensure that the asset required to support this is imported and setup.")]
        public AbstractDayNightCycle configuration;

        public float DayCycleInMinutes
        {
            get { return configuration.dayCycleInMinutes; }
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


        private void Start()
        {
            if (configuration == null)
            {
                Debug.LogError("You have not configured the Day Night Cycle.");
            }
            configuration.Initialize(startTime);
        }
        
        private void Update()
        {
            configuration.Update();
        }
    }
}
