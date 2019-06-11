using ScriptableObjectArchitecture;
using System;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.extension;

namespace wizardscode.validation
{
    /// <summary>
    /// Implementations of the AbstractSettingsSO define a desired setting for a 
    /// single value. These are used in the validation system to find the optimal
    /// settings when different plugins demand different settings.
    /// </summary>
    public abstract class AbstractSettingSO<T> : AbstractSettingSO
    {

        [Tooltip("A human readable name for this setting.")]
        public string SettingName;

        [Tooltip("Is a null value allowable? Set to true if setting can left unconfigured.")]
        public bool Nullable = false;

        [Tooltip("The suggested value for the setting. Other values may work, but if in doubt use this setting.")]
        public T m_suggestedValue;

        /// <summary>
        /// Gets the actual value of the setting in the game engine.
        /// </summary>
        protected abstract T ActualValue { get; set; }

        public virtual T SuggestedValue
        {
            get
            {
                return m_suggestedValue;
            }
        }

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
        /// Test to see if the setting is valid or not. 
        /// </summary>
        /// <returns>A ValidationResult. This will have an impact of "OK" if the setting is set to an acceptable value.</returns>
        public virtual ValidationResult Validate(Type validationTest)
        {
            ValidationResult result;
            if (!Nullable)
            {
                string testName = "Suggested Value error in " + validationTest.Name.Prettify();
                if (SuggestedValue is UnityEngine.Object)
                {
                    if (SuggestedValue as UnityEngine.Object == null)
                    {
                        result = GetErrorResult(testName, "Suggested value cannot be null.", validationTest.Name);
                        result.ReportingTest.Add(validationTest.Name);
                        return result;
                    }
                    else
                    {
                        result = GetPassResult(testName, validationTest.Name);
                    }
                }
                else if (SuggestedValue == null)
                {
                    result = GetErrorResult(testName, "Suggested value cannot be null.", validationTest.Name);
                    result.ReportingTest.Add(validationTest.Name);
                    return result;
                }
                else
                {
                    result = GetPassResult(testName, validationTest.Name);
                }
            }

            return ValidateSetting(validationTest);
        }

        /// <summary>
        /// A human readable name for the default test.
        /// </summary>
        public virtual string TestName
        {
            get { return "Default Setting Test Suite (replace this name by overriding the TestName getter in you *SettingSO)"; }
        }

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

        private ValidationResult GetResult(string testName, string message, string reportingTest, ResolutionCallback callback = null)
        {
            ValidationResult result = ValidationCollection.GetOrCreate(SettingName + " - " + testName, reportingTest);
            result.Message = message;
            result.impact = ValidationResult.Level.Warning;
            result.Callbacks = new List<ResolutionCallback>();
            if (callback != null)
            {
                result.AddCallback(callback);
            }
            else
            {
                AddDefaultFixCallback(result);
            }
            return result;
        }

        internal void AddDefaultFixCallback(ValidationResult result)
        {
            ResolutionCallback callback = new ResolutionCallback(Fix, "Automatically Resolve");
            result.AddCallback(callback);
        }

        internal ValidationResult GetErrorResult(string testName, string message, string reportingTest, ResolutionCallback callback = null)
        {
            ValidationResult result = GetResult(testName, message, reportingTest, callback);
            result.impact = ValidationResult.Level.Error;
            return result;
        }

        internal ValidationResult GetWarningResult(string testName, string message, string reportingTest, ResolutionCallback callback = null)
        {
            ValidationResult result = GetResult(testName, message, reportingTest);
            result.impact = ValidationResult.Level.Warning;
            return result;
        }

        internal ValidationResult GetPassResult(string testName, string reportingTest)
        {
            ValidationResult result = GetResult(testName, "Looks good.", reportingTest);
            result.impact = ValidationResult.Level.OK;
            return result;
        }

        #region Helpers
        /// <summary>
        /// If the candidate object is a Component return the GameObject it is attached to.
        /// If the candidate is already a Game Object return it.
        /// </summary>
        /// <param name="candidate">The object that is either a component or a GameObject</param>
        /// <returns></returns>
        internal GameObject ConvertToGameObject(UnityEngine.Object candidate)
        {
            GameObject go;
            if (candidate is Component)
            {
                go = ((Component)candidate).gameObject;
            }
            else
            {
                go = (GameObject)candidate;
            }

            return go;
        }
        #endregion
    }

    /// <summary>
    /// This AbstreactSettingSO is an empty class that is used as a convenience
    /// for reflection (it's much easier to do reflection without generics).
    /// </summary>
    public abstract class AbstractSettingSO : ScriptableObject { }

}
