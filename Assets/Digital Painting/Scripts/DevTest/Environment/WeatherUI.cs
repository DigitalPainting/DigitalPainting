using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace wizardscode.environment.test
{
    public class WeatherUI : MonoBehaviour
    {
        [Tooltip("Toggle to enable.disable auto update of the weather.")]
        public Toggle isAuto;
        [Tooltip("Dropdown for choosing precipitation type.")]
        public Dropdown precipitationTypeDropdown;

        WeatherManager manager;

        // Use this for initialization
        void Start()
        {
            manager = GameObject.FindObjectOfType<WeatherManager>();

            precipitationTypeDropdown.ClearOptions();

            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            Array types = Enum.GetValues(typeof(AbstractWeatherSystem.PrecipitationType));
            foreach (AbstractWeatherSystem.PrecipitationType type in types)
            {
                options.Add(new Dropdown.OptionData(type.ToString()));
            }
            precipitationTypeDropdown.AddOptions(options);
        }

        private void Update()
        {
            isAuto.isOn = manager.implementation.AutomaticUpdates;
            precipitationTypeDropdown.value = (int)manager.implementation.precipitationType;
        }

        public void OnPrecipitationIntensityChanged(float newValue)
        {
            manager.implementation.PrecipitationIntensity = newValue;
        }

        public void OnIsAutoEnabledChanged(bool newValue)
        {
            manager.implementation.AutomaticUpdates = newValue;
        }

        public void OnPrecipitationTypeChanged(int typeIndex)
        {
            manager.implementation.precipitationType = (AbstractWeatherSystem.PrecipitationType)Enum.GetValues(typeof(AbstractWeatherSystem.PrecipitationType)).GetValue(typeIndex);
        }
    }
}
