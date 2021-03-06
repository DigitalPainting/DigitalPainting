﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WizardsCode.Plugin
{
    public abstract class AbstractWeatherPluginDefinition : AbstractPluginDefinition
    {
        public override Type GetManagerType()
        {
            return Type.GetType("WizardsCode.Environment.WeatherPluginManager");
        }

        public override PluginCategory GetCategory()
        {
            return PluginCategory.Weather;
        }
    }
}
