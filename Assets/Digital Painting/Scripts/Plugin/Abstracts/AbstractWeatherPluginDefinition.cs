using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace wizardscode.plugin
{
    public abstract class AbstractWeatherPluginDefinition : AbstractPluginDefinition
    {
        public override Type GetManagerType()
        {
            return Type.GetType("wizardscode.environment.WeatherPluginManager");
        }
    }
}
