using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.validation
{
    [CreateAssetMenu(fileName = "SkyboxSettingSO", menuName = "Wizards Code/Validation/Skybox")]
    public class SkyBoxSettingsSO : GenericSettingSO<Material>
    {
        public override string SettingName
        {
            get { return "Skybox"; }
        }

        protected override Material ActualValue {
            get { return RenderSettings.skybox; }
            set { RenderSettings.skybox = value; }
        }
    }
}