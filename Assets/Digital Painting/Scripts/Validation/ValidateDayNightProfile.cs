using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using wizardscode.digitalpainting;
using wizardscode.editor;
using wizardscode.environment;
using wizardscode.utility;

namespace wizardscode.validation
{
    public class ValidateDayNightProfile : ValidationTest<DayNightPluginManager>
    {
        public ValidationResultCollection ExecuteOriginal()
        {
            const string PLUGIN_KEY = "Day Night Plugin";
            const string PROFILE_KEY = PLUGIN_KEY + " Profile";
            const string SKYBOX_KEY = PLUGIN_KEY + " Skybox";
            const string SUN_KEY = PLUGIN_KEY + " Sun";
            ValidationResultCollection localCollection = new ValidationResultCollection();

            /*
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
            */
            return localCollection;

        }

        /**
         * FIXME: move all loging to new SO model
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
    */
    }
}