using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace wizardscode.validation
{
    [CreateAssetMenu(fileName = "SunSettingSO", menuName = "Wizards Code/Validation/Sun Prefab")]
    public class SunSettingSO : PrefabSettingSO<Light>
    {
        protected override UnityEngine.Object ActualValue
        {
            get { return RenderSettings.sun; }
            set { RenderSettings.sun = ((GameObject)value).GetComponent<Light>() ; }
        }

        /*
        public override void Fix()
        {
            RenderSettings.sun = GetFirstInstanceInScene().GetComponent<Light>();
        }
        */
    }
}