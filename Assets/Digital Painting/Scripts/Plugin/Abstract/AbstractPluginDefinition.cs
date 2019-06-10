using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using wizardscode.digitalpainting;
using wizardscode.extension;

namespace wizardscode.plugin
{
    /// <summary>
    /// The abstract implementation of a plugin definition. All plugins should implement this class.
    /// </summary>
    public abstract class AbstractPluginDefinition
    {
        public enum PluginCategory { Agent, DayNightCycle, Weather, Other }

        /// <summary>
        /// Get the Type of the plugin manager, that is the Type of the MonoBehaviour that
        /// is used by the `DigitalPaintingManager` to manage the plugin.
        /// </summary>
        /// <returns></returns>
        public abstract Type GetManagerType();

        /// <summary>
        /// Get the name of the type of the profile for this plugin implementation. This allows
        /// us to check that a profile exists and also to check whether a plugin implementation
        /// is enabled in the scene.
        /// </summary>
        /// <returns></returns>
        public abstract String GetProfileTypeName();

        /// <summary>
        /// The name of a class that we know exists in the plugin implementation. This is used to verify that
        /// any dependent assets are installed. If the plugin is self-contained, that is it does not required
        /// another asset to function correctly then this can be set to null.
        /// </summary>
        /// <returns></returns>
        public abstract string GetPluginImplementationClassName();

        /// <summary>
        /// Get a human readable name for this type of plugin use in the UI.
        /// </summary>
        /// <returns></returns>
        public abstract PluginCategory GetCategory();

        /// <summary>
        /// Get a human readable name for use in the UI.
        /// </summary>
        /// <returns></returns>
        public abstract string GetReadableName();

        /// <summary>
        /// Get a URL from which the dependent asset can be retrieved. Normally this would be an asset store
        /// page, but it could be somewhere else, such as a GitHub repo. If the plugin is self-contained and
        /// does not depend on another asset this can be set to null.
        /// </summary>
        /// <returns>Either the URL for retrieving an asset this plugin depends upon or null if there
        /// is no such dependency.</returns>
        public abstract string GetURL();

        /// <summary>
        /// Tests to see if the plugin asset is present and ready for use. The default test
        /// is to see if the class named by `GetPluginImplmentationClassName` is available in the
        /// assembly. If no class is named then true will be returned.
        /// </summary>
        /// <returns>True if the plugin asset is present and can be used, otherwise false.</returns>
        public virtual bool AvailableForUse
        {
            get
            {
                string className = GetPluginImplementationClassName();
                if(className == null)
                {
                    // there is no dependency on external assets, so it's avaialble
                    return true;
                }
                IEnumerable<Type> types = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                           from type in assembly.GetTypes()
                                           where type.Name == className
                                           select type);
                if (types != null && types.Count() != 0)
                {
                    if (types.Count<Type>() > 1)
                    {
                        Debug.LogWarning("The PluginImplementationClassName " + GetPluginImplementationClassName() + " occurs more than once in the Assembly. We should really be checking namespaces too.");
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private DigitalPaintingManager _dpManager;
        private DigitalPaintingManager DigitalPaintingManager
        {
            get
            {
                if (_dpManager == null)
                {
                    _dpManager = GameObject.FindObjectOfType<DigitalPaintingManager>();
                }
                return _dpManager;
            }
        }


        /// <summary>
        /// Enable the plugin in your scene.
        /// </summary>
        public virtual void Enable()
        {
            GameObject go = new GameObject(GetManagerType().Name.ToString().Prettify());
            go.AddComponent(GetManagerType());
            go.transform.SetParent(DigitalPaintingManager.gameObject.transform);
        }

        public virtual void Disable()
        {
            GameObject go = DigitalPaintingManager.gameObject.GetComponentInChildren(GetManagerType()).gameObject;
            GameObject.DestroyImmediate(go);
        }
    }
}
