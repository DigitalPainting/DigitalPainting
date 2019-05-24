using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.editor;

namespace wizardscode.plugin
{
    /// <summary>
    /// A Plugin Manager is a component that is added into the scene to manage interactions between the 
    /// Digital Painting and the plugin implementation.
    /// </summary>
    public abstract class AbstractPluginManager : MonoBehaviour 
    {
        [Tooltip("The plugin profile you want to use. The profile defines the plugin implementation to use and contains the configuration.")]
        [Expandable(isRequired: true, isRequiredMessage: "Select or create a plugin profile.")]
        public AbstractPluginProfile m_pluginProfile;
    }
}
