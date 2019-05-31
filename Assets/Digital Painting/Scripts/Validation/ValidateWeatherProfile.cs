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
        
        
            /*
             * FIXME: Move to new SettingSO model
        public ValidationResultCollection Execute()
        {
            
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
            */
    }
}