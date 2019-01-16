using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.environment
{
    public abstract class AbstractDayNightCycle : ScriptableObject
    {
        [Header("Sun settings")]
        [Tooltip("A prefab containing the directional light that acts as the sun. If blank a light with the name `Sun` will be used.")]
        public Light sunPrefab;

        public const float SECOND = 1;
        public const float MINUTE_AS_SECONDS = 60 * SECOND;
        public const float HOUR_AS_SECONDS = 60 * MINUTE_AS_SECONDS;
        public const float DAY_AS_SECONDS = 24 * HOUR_AS_SECONDS;
        public const float QUARTER_DAY_AS_SECONDS = DAY_AS_SECONDS / 4;
        public const float DEGREES_PER_SECOND = 360 / DAY_AS_SECONDS;
        
        private Light _sun;
        internal Light Sun
        {
            get { return _sun; }
            set { _sun = value; }
        }

        protected DayNightCycleManager manager;

        protected float startTime;

        /// <summary>
        /// Initialize the Day Night Cycle to start at the given time.
        /// </summary>
        /// <param name="startTime">Start time in seconds.</param>
        virtual internal void Initialize(float startTime)
        {
            manager = FindObjectOfType<DayNightCycleManager>();

            this.startTime = startTime;

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
