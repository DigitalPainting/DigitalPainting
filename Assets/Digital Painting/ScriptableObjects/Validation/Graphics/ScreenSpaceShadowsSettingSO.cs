using UnityEngine;
using UnityEngine.Rendering;

namespace wizardscode.validation
{
    [CreateAssetMenu(fileName = "ScreenSpaceShadowSettingSO", menuName = "Wizards Code/Validation/Graphics/Screen Space Shadow Mode")]
    public class ScreenSpaceShadowSettingSO : GenericSettingSO<BuiltinShaderMode>
    {

        public override string SettingName
        {
            get { return "Screen Space Shadow Mode"; }
        }

        protected override BuiltinShaderMode ActualValue
        {
            get { return GraphicsSettings.GetShaderMode(BuiltinShaderType.ScreenSpaceShadows); }
            set { GraphicsSettings.SetShaderMode(BuiltinShaderType.ScreenSpaceShadows, value); }
        }
    }
}