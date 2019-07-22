using System;
using WizardsCode.Environment;

namespace WizardsCode.Plugin
{
    /// <summary>
    /// Defines the basic Day Night plugin that is included with the Digital Painting asset.
    /// </summary>
    public class SimpleDayNightPluginDefinition : AbstractDayNightPluginDefinition
    {
        public override string GetPluginImplementationClassName()
        {
            return "SimpleDayNightProfile";
        }

        public override String GetProfileTypeName()
        {
            return "SimpleDayNightProfile";
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
}
