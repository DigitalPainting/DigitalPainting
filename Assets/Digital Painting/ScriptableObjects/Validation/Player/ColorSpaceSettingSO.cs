using UnityEngine;

namespace wizardscode.validation
{
    [CreateAssetMenu(fileName = "ColorSpaceSettingSO", menuName = "Wizards Code/Validation/Player/Color Space")]
    public class ColorSpaceSettingSO : GenericSettingSO<ColorSpace>
    {
        protected override ColorSpace ActualValue
        {
            get { return UnityEditor.PlayerSettings.colorSpace; }
            set { UnityEditor.PlayerSettings.colorSpace = value; }
        }
    }
}