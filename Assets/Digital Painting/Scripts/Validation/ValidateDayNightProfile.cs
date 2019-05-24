using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using wizardscode.digitalpainting;
using wizardscode.environment;
using wizardscode.utility;

namespace wizardscode.validation
{
    public class ValidateDayNightProfile : IValidationTest
    {
        private DayNightPluginManager m_manager;

        private DayNightPluginManager Manager
        {
            get
            {
                if (m_manager == null)
                {
                    m_manager = GameObject.FindObjectOfType<DayNightPluginManager>(); ;
                }
                return m_manager;
            }
        }

        public IValidationTest Instance => new ValidateDayNightProfile();
        
        public ValidationResultCollection Execute()
        {
            const string PLUGIN_KEY = "Day Night Plugin";
            const string PROFILE_KEY = PLUGIN_KEY + " Profile";
            const string SKYBOX_KEY = PLUGIN_KEY + " Skybox";
            const string SUN_KEY = PLUGIN_KEY + " Sun";

            ValidationResultCollection localCollection = new ValidationResultCollection();

            ValidationResult result;

            // Is plugin enabled
            if (Manager == null)
            {
                result = ValidationHelper.Validations.GetOrCreate(PLUGIN_KEY);
                result.Message = "Day Night Plugin not enabled (click ignore if you don't want to use it)";
                result.impact = ValidationResult.Level.Warning;
                result.resolutionCallback = EnableDayNightPlugin;
                localCollection.AddOrUpdate(result);

                ValidationHelper.Validations.Remove(PROFILE_KEY);
                ValidationHelper.Validations.Remove(SKYBOX_KEY);
                ValidationHelper.Validations.Remove(SUN_KEY);

                return localCollection;
            } else
            {
                ValidationHelper.Validations.Remove(PLUGIN_KEY);
            }
            
            // Has profile?
            if (Manager.Profile == null)
            {
                result = ValidationHelper.Validations.GetOrCreate(PROFILE_KEY);
                result.Message = "Day Night Plugin is enabled, but there is no profile assigned to it.";
                result.impact = ValidationResult.Level.Error;
                result.resolutionCallback = SelectDayNightPluginManager;
                localCollection.AddOrUpdate(result);
                return localCollection;
            } else
            {
                ValidationHelper.Validations.Remove(PROFILE_KEY);
            }
            
            // Skybox setup correctly?
            if (Manager.Profile.skybox == null)
            {
                result = ValidationHelper.Validations.GetOrCreate(SKYBOX_KEY);
                result.Message = "No skybox is defined in the Day Night Profile.";
                result.impact = ValidationResult.Level.Warning;
                result.resolutionCallback = SelectDayNightPluginManager;
                localCollection.AddOrUpdate(result);
            }
            else if (RenderSettings.skybox != Manager.Profile.skybox)
            {
                result = ValidationHelper.Validations.GetOrCreate(SKYBOX_KEY);
                result.Message = "Skybox set in RenderSettings is not the same as the one set in the Day Night Profile";
                result.impact = ValidationResult.Level.Error;
                result.resolutionCallback = SetSkybox;
                localCollection.AddOrUpdate(result);
            }
            else
            {
                ValidationHelper.Validations.Remove(SKYBOX_KEY);
            }

            // Sun setup correctly?

            if (Manager.Profile.sunPrefab == null)
            {
                result = ValidationHelper.Validations.GetOrCreate(SUN_KEY);
                result.Message = "No sun prefab is set in the Day Night Profile";
                result.impact = ValidationResult.Level.Warning;
                result.resolutionCallback = SelectDayNightPluginManager;
                localCollection.AddOrUpdate(result);
            }
            else
            {
                ValidationHelper.Validations.Remove(SUN_KEY);
            }

            return localCollection;
        }

        private void EnableDayNightPlugin()
        {
            GameObject dpManager = GameObject.FindObjectOfType<DigitalPaintingManager>().gameObject;
            dpManager.AddComponent(typeof(DayNightPluginManager));
            SelectDayNightPluginManager();
        }

        void SelectDayNightPluginManager()
        {
            Selection.activeGameObject = Manager.gameObject;
        }

        /// <summary>
        /// Set the RenderSettings.skybox to that set in the profile.
        /// </summary>
        void SetSkybox()
        {
            RenderSettings.skybox = Manager.Profile.skybox;
        }

        void AddSun()
        {
            Light sun = GameObject.Instantiate(Manager.Profile.sunPrefab);
            RenderSettings.sun = sun;
        }
    }
}