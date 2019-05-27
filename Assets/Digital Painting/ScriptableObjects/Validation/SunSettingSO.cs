using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.validation
{
    [CreateAssetMenu(fileName = "SunSettingSO", menuName = "Wizards Code/Validation/Sun Setting")]
    public class SunSettingsSO : GenericSettingSO<Light>
    {
        public override string Name
        {
            get { return "Sun"; }
        }

        protected override Light ActualValue
        {
            get { return RenderSettings.sun; }
            set { RenderSettings.sun = value; }
        }

        public override void Fix()
        {
            Light sun = GameObject.Instantiate(SuggestedValue);
            RenderSettings.sun = sun;
        }
    }
}