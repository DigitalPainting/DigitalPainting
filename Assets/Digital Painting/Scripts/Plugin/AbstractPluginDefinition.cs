using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The abstract implementation of a plugin definition. All plugins should implement this class.
/// </summary>
public abstract class AbstractPluginDefinition
{
    /// <summary>
    /// Get the type of the manager, that is the Type of the MonoBehaviour that
    /// is used by the `DigitalPaintingManager` to manage the plugin.
    /// </summary>
    /// <returns></returns>
    public abstract Type GetManagerType();

    /// <summary>
    /// The name of a class that we know exists in the plugin implementation.
    /// </summary>
    /// <returns></returns>
    public abstract string GetPluginImplementationClassName();

    /// <summary>
    /// Get a human readable name for use in the UI.
    /// </summary>
    /// <returns></returns>
    public abstract string GetReadableName();

    /// <summary>
    /// Get a URL from which this asset can be retrieved. Normally this would be an asset store
    /// page, but it could be somewhere else, such as a GitHub repo.
    /// </summary>
    /// <returns></returns>
    public abstract string GetURL();

    /// <summary>
    /// Tests to see if the plugin asset is present and ready for use. The default test
    /// is to see if the class named in `PluginImplmentationClassName` is available in the
    /// assembly.
    /// </summary>
    /// <returns>True if the plugin asset is present and can be used, otherwise false.</returns>
    public virtual bool IsPresent {
        get
        {
            string className = GetPluginImplementationClassName();
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
}
