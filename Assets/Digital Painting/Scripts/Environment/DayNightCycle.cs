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
    public class DayNightCycle : MonoBehaviour
    {
        [Tooltip("Start time in seconds. 0 and 86400 is midnight.")]
        [Range(0, 86400)]
        public float startTime = 5 * 60 * 60; // (5 AM)

        [Tooltip("The Day Night Cycle configuration you want to use. Ensure that the asset required to support this is imported and setup.")]
        public AbstractDayNightCycle configuration;

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
