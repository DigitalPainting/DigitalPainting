using UnityEngine;

namespace wizardscode.validation
{
    [CreateAssetMenu(fileName = "ShadowDistanceSettingSO", menuName = "Wizards Code/Validation/Quality/Shadow Distance")]
    public class ShadowDistanceSettingSO : GenericSettingSO<float>
    {

        public override string SettingName
        {
            get { return "Shadow Distance"; }
        }

        protected override float ActualValue
        {
            get { return QualitySettings.shadowDistance; }
            set { QualitySettings.shadowDistance = value; }
        }
    }
}