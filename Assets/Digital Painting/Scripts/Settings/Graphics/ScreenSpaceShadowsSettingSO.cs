using UnityEngine;
using UnityEngine.Rendering;

namespace WizardsCode.Validation
{
    [CreateAssetMenu(fileName = "ScreenSpaceShadow_SettingSO_DESCRIPTIVENAME", menuName = "Wizards Code/Validation/Graphics/Screen Space Shadow Mode")]
    public class ScreenSpaceShadowsSettingSO : GenericSettingSO<BuiltinShaderMode>
    {
        protected override BuiltinShaderMode ActualValue
        {
            get { return GraphicsSettings.GetShaderMode(BuiltinShaderType.ScreenSpaceShadows); }
            set { GraphicsSettings.SetShaderMode(BuiltinShaderType.ScreenSpaceShadows, value); }
        }
    }
}