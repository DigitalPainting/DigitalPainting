using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace wizardscode.validation
{
    [CreateAssetMenu(fileName = "SunSettingSO", menuName = "Wizards Code/Validation/Sun Prefab")]
    public class SunSettingSO : GenericSettingSO<Light>
    {
        protected override Light ActualValue
        {
            get { return RenderSettings.sun; }
            set { RenderSettings.sun = value; }
        }

        public override void Fix()
        {
            RenderSettings.sun = GetFirstInstanceInScene().GetComponent<Light>();
        }
    }
}