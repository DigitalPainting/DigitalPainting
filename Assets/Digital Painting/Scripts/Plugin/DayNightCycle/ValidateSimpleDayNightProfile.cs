using System;
using WizardsCode.environment;

namespace WizardsCode.validation
{
    public class ValidateSimpleDayNightProfile : ValidationTest<DayNightPluginManager>
    {
        public override ValidationTest<DayNightPluginManager> Instance => new ValidateSimpleDayNightProfile();

        internal override Type ProfileType => typeof(SimpleDayNightProfile);
    }
}