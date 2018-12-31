using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using wizardscode.environment;

namespace wizardscode.digitalpainting.devtest {
    public class DayNightCycleUI : MonoBehaviour {
        [Tooltip("Text label to display name of the Day Night Cycle Implementation being used.")]
        public Text implementatioNameLabel;
        [Tooltip("Slider to set current time of day.")]
        public Slider timeOfDaySlider;
        [Tooltip("Time of Day text object.")]
        public Text timeOfDayLabel;
        [Tooltip("Label for speed of day.")]
        public Text dayLengthLabel;

        [Header("Overrides")]
        [Tooltip("The DayNightCycle object that we are using. If null then the first one found in the scene will be used. This is usually sufficient.")]
        public DayNightCycleManager dayNightCycle;

        private void Start()
        {
            dayNightCycle = GameObject.FindObjectOfType<DayNightCycleManager>();
        }

        private void Update()
        {
            implementatioNameLabel.text = dayNightCycle.ImplementationName;
            timeOfDayLabel.text = "Time: " + dayNightCycle.CurrentTimeAsLabel;
            dayLengthLabel.text = "Minutes per sim. day: " + dayNightCycle.DayCycleInMinutes;
        }
        
        public void OnTimeOfDayChanged()
        {
            dayNightCycle.configuration.SetTime(timeOfDaySlider.value);
        }
    }
}
