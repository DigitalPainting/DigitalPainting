using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.utility;

public class ValidateShadowsConfiguration : IValidationTest
{
    public IValidationTest Instance => new ValidateShadowsConfiguration();

    public ValidationResultCollection Execute()
    {
        ValidationResultCollection localCollection = new ValidationResultCollection();

        ValidationResult result = ValidationHelper.Validations.GetOrCreate("Shadows");
        if (QualitySettings.shadowDistance >= 500)
        {
            result.impact = ValidationResult.Level.OK;
        }
        else
        {
            result.Message = "Shadows are not setup in the recommended way.";
            result.impact = ValidationResult.Level.Warning;
            result.resolutionCallback = ConfigureShadows;
        }
        return localCollection;
    }

    private void ConfigureShadows()
    {
        QualitySettings.shadowDistance = 500;
    }
}
