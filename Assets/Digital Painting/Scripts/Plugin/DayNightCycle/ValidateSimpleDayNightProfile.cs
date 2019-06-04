using wizardscode.environment;

namespace wizardscode.validation
{
    public class ValidateSimpleDayNightProfile : ValidationTest<DayNightPluginManager>
    {
        public override ValidationTest<DayNightPluginManager> Instance => new ValidateSimpleDayNightProfile();

        internal override string ProfileType { get { return "SimpleDayNightProfile"; } }
    }
}