using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.environment
{
    [CreateAssetMenu(fileName = "DummyWeatherSystem", menuName = "Wizards Code/Weather/Dummy Weather System (for testing only)")]
    public class DummyWeatherSystem : AbstractWeatherProfile
    {   
        internal override void Initialize()
        {
        }

        internal override void Start()
        {
            
        }

        override internal void Update()
        {
            string summary = CurrentProfile.PrecipitationType + ". Time since start: " + Time.realtimeSinceStartup;
                
            Debug.Log("Current weather report: " + summary + "\n" + CurrentProfile.ToString());
        }
    }
}