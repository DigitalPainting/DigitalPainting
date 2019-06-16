using System;
using wizardscode.environment;

namespace wizardscode.validation
{
    public class ValidateSimpleDayNightProfile : ValidationTest<DayNightPluginManager>
    {
        public override ValidationTest<DayNightPluginManager> Instance => new ValidateSimpleDayNightProfile();

        internal override Type ProfileType => typeof(SimpleDayNightProfile);
    }
}