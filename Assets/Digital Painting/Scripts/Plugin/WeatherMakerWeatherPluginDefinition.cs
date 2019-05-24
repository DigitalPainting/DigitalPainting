using System;


namespace wizardscode.plugin
{
    public class WeatherMakerWeatherPluginDefinition : AbstractWeatherPluginDefinition
    {
        public override string GetPluginImplementationClassName()
        {
            return "WeatherMakerScript";
        }

        public override String GetProfileTypeName()
        {
            return "WeatherMakerWeatherProfile";
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
}
