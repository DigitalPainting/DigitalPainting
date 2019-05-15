using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractWeatherPluginDefinition : AbstractPluginDefinition
{
    public override Type GetManagerType()
    {
        return Type.GetType("wizardscode.environment.WeatherManager");
    }
}
