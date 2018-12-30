using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.environment
{
    public abstract class AbstractDayNightCycle : ScriptableObject
    {
        [Header("Timing")]
        [Tooltip("The speed at which a game day passes in realtime.")]
        public float dayCycleInMinutes = 1;

        [Header("Sun settings")]
        [Tooltip("A prefab containing the directional light that acts as the sun. If blank a light with the name `Sun` will be used.")]
        public Light sunPrefab;

        [Header("Lighting Settings")]
        [Tooltip("Skybox material")]
        public Material skyboxMaterial;

        public const float SECOND = 1;
        public const float MINUTE_AS_SECONDS = 60 * SECOND;
        public const float HOUR_AS_SECONDS = 60 * MINUTE_AS_SECONDS;
        public const float DAY_AS_SECONDS = 24 * HOUR_AS_SECONDS;
        public const float DEGREES_PER_SECOND = 360 / DAY_AS_SECONDS;

        protected float currentTimeOfDay;

        private Light _sun;
        internal Light Sun
        {
            get { return _sun; }
            set { _sun = value; }
        }


        /// <summary>
        /// Initialize the Day Night Cycle to start at the given time.
        /// </summary>
        /// <param name="startTime">Start time in seconds.</param>
        internal void Initialize(float startTime)
        {
            currentTimeOfDay = startTime;
            InitializeSun();
            InitializeSkybox();
        }

        abstract internal void InitializeSun();

        abstract internal void InitializeSkybox();

        abstract internal void Update();
    }
}
