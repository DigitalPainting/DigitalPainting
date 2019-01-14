﻿using System;
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
        [Tooltip("The UI elements to disable when Is Auto is on (checked).")]
        public GameObject manualControls;

        WeatherManager manager;

        // Use this for initialization
        void Start()
        {
            manager = GameObject.FindObjectOfType<WeatherManager>();
            if (manager == null)
            {
                gameObject.SetActive(false);
                return;
            }

            precipitationTypeDropdown.ClearOptions();

            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            Array types = Enum.GetValues(typeof(WeatherProfile.PrecipitationTypeEnum));
            foreach (WeatherProfile.PrecipitationTypeEnum type in types)
            {
                options.Add(new Dropdown.OptionData(type.ToString()));
            }
            precipitationTypeDropdown.AddOptions(options);
        }

        private void Update()
        {
            isAuto.isOn = manager.isAuto;            
            manualControls.SetActive(!isAuto.isOn);

            if (!isAuto.isOn)
            {
                precipitationTypeDropdown.value = (int)manager.configuration.CurrentProfile.PrecipitationType;
            }
        }

        public void OnPrecipitationIntensityChanged(float newValue)
        {
            manager.configuration.CurrentProfile.PrecipitationIntensity = newValue;
            manager.UpdateNow();
        }

        public void OnIsAutoEnabledChanged(bool newValue)
        {
            manager.isAuto = newValue;
        }

        public void OnPrecipitationTypeChanged(int typeIndex)
        {
            manager.configuration.CurrentProfile.PrecipitationType = (WeatherProfile.PrecipitationTypeEnum)Enum.GetValues(typeof(WeatherProfile.PrecipitationTypeEnum)).GetValue(typeIndex);
            manager.UpdateNow();
        }
    }
}