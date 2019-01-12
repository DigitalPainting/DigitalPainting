using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.environment
{
    /// <summary>
    /// The configuration for the scenes Weather implementation.
    /// This is used to define which Weather asset is being used
    /// and to set the basic configuration, such as current weather. Finer
    /// control is managed through your chosen asset.
    /// </summary>
    [AddComponentMenu("Wizards Code/Environment/Weather")]
    public class WeatherManager : MonoBehaviour
    {

        [Tooltip("The Weather configuration you want to use. Ensure that the asset required to support this is imported and setup.")]
        public AbstractWeatherSystem implementation;

        private void Awake()
        {
            implementation.Initialize();
        }

        private void Start()
        {
            implementation.Start();
        }

        private void Update()
        {
            implementation.Update();
        }
    }
}
