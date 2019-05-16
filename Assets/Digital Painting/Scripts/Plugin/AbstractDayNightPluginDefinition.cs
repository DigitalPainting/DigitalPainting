using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace wizardscode.plugin
{
    /// <summary>
    /// An abstract definition for a Day Night Cycle plugin.
    /// These plugins manage the time of day and associated skybox and lighting.
    /// </summary>
    public abstract class AbstractDayNightPluginDefinition : AbstractPluginDefinition
    {
        public override Type GetManagerType()
        {
            return Type.GetType("wizardscode.environment.DayNightPluginManager");
        }
    }
}
