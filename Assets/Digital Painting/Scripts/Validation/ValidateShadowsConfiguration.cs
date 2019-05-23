using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.utility;

public class ValidateShadowsConfiguration : ValidationTest
{
    public static ValidateShadowsConfiguration Instance = new ValidateShadowsConfiguration();

    public override ValidationResult Execute()
    {
        ValidationResult result = ValidationHelper.Validations.GetOrCreate("Shadows are correctly setup.");
        if (QualitySettings.shadowDistance >= 500)
        {
            result.impact = ValidationResult.Level.OK;
        }
        else
        {
            result.impact = ValidationResult.Level.Warning;
            result.resolutionCallback = ConfigureShadows;
        }
        return result;
    }

    private void ConfigureShadows()
    {
        QualitySettings.shadowDistance = 500;
    }
}
