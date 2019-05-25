using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.utility;

namespace wizardscode.digitalpainting
{
    public class ValidateQualityConfiguration : IValidationTest
    {
        const string MAIN_KEY = "Digital Painting";
        const string QUALITY_KEY = MAIN_KEY + " Quality Settings";

        public IValidationTest Instance => new ValidateQualityConfiguration();

        public ValidationResultCollection Execute()
        {
            ValidationResultCollection localCollection = new ValidationResultCollection();
            ValidationResult result;

            if (QualitySettings.shadowDistance < 500)
            {
                result = ValidationHelper.Validations.GetOrCreate(QUALITY_KEY);
                result.Message = "Shadows are not setup in the recommended way.";
                result.impact = ValidationResult.Level.Warning;
                result.resolutionCallback = ConfigureShadows;
                localCollection.AddOrUpdate(result);
            } else
            {
                ValidationHelper.Validations.Remove(QUALITY_KEY);
            }


            if (UnityEditor.PlayerSettings.colorSpace != ColorSpace.Linear)
            {
                result = ValidationHelper.Validations.GetOrCreate(QUALITY_KEY);
                result.Message = "Color space is better as Linear if your target platform can handle it.";
                result.impact = ValidationResult.Level.Warning;
                result.resolutionCallback = SetColorSpaceToLinear;
                localCollection.AddOrUpdate(result);
            }
            else
            {
                ValidationHelper.Validations.Remove(QUALITY_KEY);
            }

            return localCollection;
        }

        private void ConfigureShadows()
        {
            QualitySettings.shadowDistance = 500;
        }

        private void SetColorSpaceToLinear()
        {
            UnityEditor.PlayerSettings.colorSpace = ColorSpace.Linear;
        }
    }
}