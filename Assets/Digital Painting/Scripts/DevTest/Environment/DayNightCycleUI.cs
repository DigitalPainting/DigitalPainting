using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using wizardscode.environment;

namespace wizardscode.digitalpainting.devtest {
    public class DayNightCycleUI : MonoBehaviour {
        [Tooltip("Text label to display name of the Day Night Cycle Implementation being used.")]
        public Text implementatioNameLabel;
        [Tooltip("Label for current phase of the day.")]
        public Text phaseLabel;
        [Tooltip("Slider to set current time of day.")]
        public Slider timeOfDaySlider;
        [Tooltip("Time of Day text object.")]
        public Text timeOfDayLabel;
        [Tooltip("Label for speed of day.")]
        public Text dayLengthLabel;

        [Header("Overrides")]
        [Tooltip("The DayNightCycle object that we are using. If null then the first one found in the scene will be used. This is usually sufficient.")]
        public DayNightPluginManager dayNightManager;

        private void Start()
        {
            dayNightManager = GameObject.FindObjectOfType<DayNightPluginManager>();
        }

        private void Update()
        {
            if (dayNightManager != null && dayNightManager.enabled) { 
                implementatioNameLabel.text = dayNightManager.ImplementationName;
                phaseLabel.text = dayNightManager.CurrentPhase.ToString();
                timeOfDayLabel.text = "Time: " + dayNightManager.CurrentTimeAsLabel;
                dayLengthLabel.text = "Minutes per sim. day: " + dayNightManager.DayCycleInMinutes;
            } else
            {
                implementatioNameLabel.text = "Day Night Cycle disabled";
                phaseLabel.text = "Phase: N/A";
                timeOfDayLabel.text = "Time: N/A";
                dayLengthLabel.text = "Minutes per sim. day: N/A";
            }
        }
        
        public void OnTimeOfDayChanged()
        {
            dayNightManager.DayNightProfile.SetTime(timeOfDaySlider.value);
        }
    }
}
