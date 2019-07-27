using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WizardsCode.Editor;
using WizardsCode.Extension;

namespace WizardsCode.Plugin
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

        public AbstractPluginProfile Profile
        {
            get { return m_pluginProfile; }
            internal set { m_pluginProfile = value; }
        }
    }
}
