using System;
using WizardsCode.Environment;

namespace WizardsCode.Validation
{
    public class ValidateSimpleDayNightProfile : ValidationTest<DayNightPluginManager>
    {
        public override ValidationTest<DayNightPluginManager> Instance => new ValidateSimpleDayNightProfile();

        internal override Type ProfileType => typeof(SimpleDayNightProfile);
    }
}