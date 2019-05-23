using System.Collections.Generic;
using UnityEngine;
using wizardscode.utility;

namespace wizardscode.plugin
{
    public abstract class AbstractPluginProfile : ScriptableObject
    {
        /// <summary>
        /// Test to see if the plugin profile is compatible with the current scene. 
        /// </summary>
        /// <returns>A list of ValidationResults that describe any problems found. If the list is empty then no errors were found.</returns>
        public virtual List<ValidationResult> Validate()
        {
            return new List<ValidationResult>();
        }
    }
}
