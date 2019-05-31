using UnityEditor;
using UnityEngine;
using wizardscode.digitalpainting;
using wizardscode.environment;
using wizardscode.utility;

namespace wizardscode.validation
{
    public class ValidateWeatherProfile : ValidationTest<WeatherPluginManager>
    {

        public override ValidationTest<WeatherPluginManager> Instance => new ValidateWeatherProfile();

        internal override string ProfileType { get { return "WeatherProfile"; } }

        private WeatherPluginManager m_manager;

        private WeatherPluginManager Manager
        {
            get
            {
                if (m_manager == null)
                {
                    m_manager = GameObject.FindObjectOfType<WeatherPluginManager>(); ;
                }
                return m_manager;
            }
        }
    }
}