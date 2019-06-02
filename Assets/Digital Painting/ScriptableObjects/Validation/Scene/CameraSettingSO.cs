using UnityEngine;

namespace wizardscode.validation
{
    [CreateAssetMenu(fileName = "CameraSettingSO", menuName = "Wizards Code/Validation/Scene/Camera")]
    public class CameraSettingSO : PrefabSettingSO<Camera>
    {
        protected override UnityEngine.Object ActualValue
        {
            get { return Camera.main; }
            set { ((GameObject)Instance).tag = "MainCamera"; }
        }
    }
}