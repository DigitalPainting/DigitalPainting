using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.plugin
{
    /// <summary>
    /// A Plugin Manager is a component that is added into the scene to manage interactions between the 
    /// Digital Painting and the plugin implementation.
    /// </summary>
    public abstract class AbstractPluginManager : MonoBehaviour 
    {
        [Tooltip("The Day Night Cycle configuration you want to use. Ensure that the asset required to support this is imported and setup.")]
        public AbstractPluginProfile m_pluginProfile;
    }
}
