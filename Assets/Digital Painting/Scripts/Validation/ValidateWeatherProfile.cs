using UnityEditor;
using UnityEngine;
using wizardscode.digitalpainting;
using wizardscode.environment;
using wizardscode.utility;

namespace wizardscode.validation
{
    public class ValidateWeatherProfile : IValidationTest
    {
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

        public IValidationTest Instance => new ValidateWeatherProfile();
        
        public ValidationResultCollection Execute()
        {
            const string PLUGIN_KEY = "Weather Plugin";
            const string PROFILE_KEY = PLUGIN_KEY + " Profile";

            ValidationResultCollection localCollection = new ValidationResultCollection();
            ValidationResult result;

            // Is plugin enabled
            if (Manager == null)
            {
                result = ValidationHelper.Validations.GetOrCreate(PLUGIN_KEY);
                result.Message = "Weather Plugin not enabled (click ignore if you don't want to use it)";
                result.impact = ValidationResult.Level.Warning;
                result.resolutionCallback = EnableWeatherPlugin;
                localCollection.AddOrUpdate(result);

                ValidationHelper.Validations.Remove(PROFILE_KEY);

                return localCollection;
            } else
            {
                ValidationHelper.Validations.Remove(PLUGIN_KEY);
            }
            
            // Has profile?
            if (Manager.Profile == null)
            {
                result = ValidationHelper.Validations.GetOrCreate(PROFILE_KEY);
                result.Message = "Weather Plugin is enabled, but there is no profile assigned to it.";
                result.impact = ValidationResult.Level.Error;
                result.resolutionCallback = SelectWeatherPluginManager;
                localCollection.AddOrUpdate(result);
                return localCollection;
            } else
            {
                ValidationHelper.Validations.Remove(PROFILE_KEY);
            }
            
            return localCollection;
        }

        private void EnableWeatherPlugin()
        {
            GameObject dpManager = GameObject.FindObjectOfType<DigitalPaintingManager>().gameObject;
            dpManager.AddComponent(typeof(WeatherPluginManager));
            SelectWeatherPluginManager();
        }

        void SelectWeatherPluginManager()
        {
            Selection.activeGameObject = Manager.gameObject;
        }
    }
}