using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.plugin;
using wizardscode.utility;
using wizardscode.validation;

namespace wizardscode.digitalpainting
{
    public class ValidateQualityConfiguration : ValidationTest<AbstractPluginManager>
    {

        public override ValidationTest<AbstractPluginManager> Instance => new ValidateQualityConfiguration();

        internal override string ProfileType { get { return "FIXME: QA Config does not have a profile."; } }

        /*
         * FIXME: move to the new settings SO model
        const string MAIN_KEY = "Digital Painting";
        const string QUALITY_KEY = MAIN_KEY + " Quality Settings";


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
        */
    }
}