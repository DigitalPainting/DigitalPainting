using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherMakerPluginDefinition : AbstractWeatherPluginDefinition
{
    public override string GetPluginImplementationClassName()
    {
        return "WeatherMakerWeatherSystem";
    }

    public override string GetReadableName()
    {
        return "Weather Maker";
    }

    public override string GetURL()
    {
        return "https://assetstore.unity.com/packages/tools/particles-effects/weather-maker-unity-weather-system-sky-water-volumetric-clouds-a-60955";
    }
}
