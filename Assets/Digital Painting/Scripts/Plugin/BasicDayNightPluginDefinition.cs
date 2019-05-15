using System;
/// <summary>
/// Defines the basic Day Night plugin that is included with the Digital Painting asset.
/// </summary>
public class BasicDayNightPluginDefinition : AbstractDayNightPluginDefinition
{
    public override string GetPluginImplementationClassName()
    {
        return "SimpleDayNightCycle";
    }

    public override string GetReadableName()
    {
        return "Simple Day Night Cycle";
    }

    public override string GetURL()
    {
        return "https://github.com/DigitalPainting/DigitalPainting";
    }
}
