using UnityEngine;
using wizardscode.plugin;

namespace wizardscode.environment
{
    /// <summary>
    /// Describes the common configuration of a Day Night plugin.
    /// </summary>
    public abstract class AbstractDayNightProfile : AbstractPluginProfile
    {
        [Header("Environment settings")]
        [Tooltip("Skybox materials to use.")]
        public Material skybox;
        [Tooltip("A prefab containing the directional light that acts as the sun. If blank a light with the name `Sun` will be used.")]
        public Light sunPrefab;

        [Header("Timing")]
        [Tooltip("Start time in seconds. 0 and 86400 is midnight.")]
        [Range(0, 86400)]
        public float startTime = 5 * 60 * 60; // (5 AM)
        [Tooltip("The speed at which a game day passes in real-time.")]
        public float dayCycleInMinutes = 1;

        protected DayNightPluginManager manager;

        private Light _sun;
        internal Light Sun
        {
            get { return _sun; }
            set
            {
                if (value != _sun)
                {
                    _sun = value;
                    RenderSettings.sun = _sun;
                }
            }
        }

        public void AddSun()
        {
            Light sun = Instantiate(sunPrefab);
            RenderSettings.sun = sun;
        }

        /// <summary>
        /// Initialize the Day Night Cycle to start at the given time.
        /// </summary>
        /// <param name="startTime">Start time in seconds.</param>
        virtual internal void Initialize()
        {
            manager = GameObject.FindObjectOfType<DayNightPluginManager>();
            if (manager == null)
            {
                Debug.LogError("Cannot find DayNightCycleManager.");
                return;
            }

            InitializeTiming();
            InitializeCamera();
            InitializeSun();
            InitializeLighting();
        }

        /// <summary>
        /// Get the current time.
        /// </summary>
        /// <returns>Current time in seconds.</returns>
        abstract internal float GetTime();

        abstract internal void SetTime(float timeInSeconds);

        abstract internal void InitializeCamera();

        abstract internal void InitializeLighting();

        abstract internal void InitializeTiming();

        abstract internal void InitializeSun();

        abstract internal void Update();
    }

}
