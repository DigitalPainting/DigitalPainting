using UnityEngine;

namespace wizardscode.validation
{
    [CreateAssetMenu(fileName = "CameraSettingSO", menuName = "Wizards Code/Validation/Scene/Camera")]
    public class CameraSettingSO : GenericSettingSO<Camera>
    {
        public override string TestName
        {
            get { return "Configuration"; }
        }

        public override string SettingName { get { return "Weather Maker Camera"; } }

        protected override Camera ActualValue
        {
            get { return Camera.main; }
            set { GetFirstInstanceInScene().tag = "MainCamera"; }
        }
    }
}