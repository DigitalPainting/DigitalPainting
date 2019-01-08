using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace wizardscode.environment.test
{
    public class WeatherUI : MonoBehaviour
    {
        [Tooltip("Dropdown for coosing precipitation type")]
        public Dropdown precipitationTypeDropdown;

        WeatherManager manager;

        // Use this for initialization
        void Start()
        {
            manager = GameObject.FindObjectOfType<WeatherManager>();

            precipitationTypeDropdown.ClearOptions();

            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            options.Add(new Dropdown.OptionData())
            precipitationTypeDropdown.AddOptions(options);
        }

        public void OnPrecipitationIntensityChanged(float newValue)
        {
            manager.implementation.PrecipitationIntensity = newValue;
        }

        public void OnIsAutoEnabledChanged(bool newValue)
        {
            manager.implementation.AutomaticUpdates = newValue;
        }
    }
}
