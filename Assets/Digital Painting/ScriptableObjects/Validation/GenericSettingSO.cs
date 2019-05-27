using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using wizardscode.utility;

namespace wizardscode.validation
{
    public abstract class GenericSettingSO<T> : AbstractSettingSO
    {
        /// <summary>
        /// A GenericSettingsSO defines a desired setting for a 
        /// single value. These are used in the validation system to find the optimal
        /// settings when different plugins demand different settings.
        /// </summary>
        [Tooltip("The suggested value for the setting. Other values may work, but if in doubt use this setting.")]
        public T SuggestedValue;

        protected abstract T ActualValue { get; set;  }

        public override ValidationResult Validate()
        {
            ValidationResult result;
            if (!Nullable)
            {
                string testName = "Suggested Value";
                if (SuggestedValue is UnityEngine.Object)
                {
                    if (SuggestedValue as UnityEngine.Object == null)
                    {
                        result = GetErrorResult(testName, "Suggested value cannot be null.");
                        return result;
                    } else
                    {
                        result = GetPassResult(testName);
                    }
                } else if (SuggestedValue == null)
                {
                    result = GetErrorResult(testName, "Suggested value cannot be null.");
                    return result;
                }
                else
                {
                    result = GetPassResult(testName);
                }
            }
            return ValidateSetting();
        }

        public override void Fix()
        {
            ActualValue = SuggestedValue;
        }

        internal override ValidationResult ValidateSetting()
        {
            ValidationResult result = null;

            // Actual Value is correctly set
            if (!object.Equals(ActualValue, SuggestedValue))
            {
                result = GetWarningResult("Setting Value", "The value set is not the same as the suggested value. This may be OK, in which case click the ignore checkbox.");
            }

            if (result == null)
            {
                result = new ValidationResult("Full Suite", ValidationResult.Level.OK);
            }
            return result;
        }
    }
}
