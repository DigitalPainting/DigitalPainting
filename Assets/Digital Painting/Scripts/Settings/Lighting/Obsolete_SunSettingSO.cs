using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WizardsCode.Validation
{
    [Obsolete("Use PrefabSettingSO instead")]
    public class SunSettingSO : PrefabSettingSO
    {

        /*
        protected override UnityEngine.Object ActualValue
        {
            get { return RenderSettings.sun; }
            set { RenderSettings.sun = ((GameObject)value).GetComponent<Light>() ; }
        }

        public override void Fix()
        {
            RenderSettings.sun = GetFirstInstanceInScene().GetComponent<Light>();
        }
        */
    }
}