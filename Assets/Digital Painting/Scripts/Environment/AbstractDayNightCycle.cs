using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.environment
{
    public abstract class AbstractDayNightCycle : ScriptableObject
    {
        protected DayNightCycleManager manager;

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

        protected float startTime;

        /// <summary>
        /// Initialize the Day Night Cycle to start at the given time.
        /// </summary>
        /// <param name="startTime">Start time in seconds.</param>
        virtual internal void Initialize(float startTime)
        {
            this.startTime = startTime;

            manager = GameObject.FindObjectOfType<DayNightCycleManager>();
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
