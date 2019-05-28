using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using wizardscode.utility;

namespace wizardscode.validation
{
    /// <summary>
    /// Implementations of the AbstractSettingsSO define a desired setting for a 
    /// single value. These are used in the validation system to find the optimal
    /// settings when different plugins demand different settings.
    /// </summary>
    public abstract class AbstractSettingSO : ScriptableObject
    {
        [Tooltip("A description of the setting and why it should be set this way.")]
        public string Description;

        [Tooltip("Is a null value allowable? Set to true if setting can left unconfigured.")]
        public bool Nullable = false;

        [SerializeField]
        private ValidationResultCollection validationCollection;

        private ValidationResultCollection ValidationCollection
        {
            get
            {
                if (validationCollection == null)
                {
                    validationCollection = new ValidationResultCollection(); ;
                }
                return validationCollection;
            }
        }

        /// <summary>
        /// A human readable name for this setting.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Test to see if the setting is valid or not. 
        /// </summary>
        /// <returns>A ValidationResult. This will have an impact of "OK" if the setting is set to an acceptable value.</returns>
        public abstract ValidationResult Validate(Type validationTest);

        /// <summary>
        /// Test to see if the setting is valid or not. It's not necessary to test for null values here, 
        /// that is done automatically.
        /// </summary>
        /// <returns>True or false depending on whether the setting is correctly set (true) or not (false)</returns>
        internal abstract ValidationResult ValidateSetting(Type validationTest);

        /// <summary>
        /// This method will be executed when the user clicks a button to automatically fix the setting.
        /// In some cases this will simply set the recommended setting, in other cases this will take the
        /// user to the setting to fix it themselves.
        /// </summary>
        public abstract void Fix();

        private ValidationResult GetResult(string testName, string message)
        {
            ValidationResult result = ValidationCollection.GetOrCreate(Name + " - " + testName);
            result.Message = message;
            result.impact = ValidationResult.Level.Warning;
            result.Callback = new ResolutionCallback(Fix, "Automatically Resolve");
            return result;
        }

        internal ValidationResult GetErrorResult(string testName, string message)
        {
            ValidationResult result = GetResult(testName, message);
            result.impact = ValidationResult.Level.Error;
            return result;
        }

        internal ValidationResult GetWarningResult(string testName, string message)
        {
            ValidationResult result = GetResult(testName, message);
            result.impact = ValidationResult.Level.Warning;
            return result;
        }

        internal ValidationResult GetPassResult(string testName)
        {
            ValidationResult result = GetResult(testName, "Looks good.");
            result.impact = ValidationResult.Level.OK;
            return result;
        }
    }
}
