using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Build the package for plugin release. To implement this class for a package simply provide the
/// package name in the GetPluginName method implementation and uncomment the MenuItem line, editing as
/// appropriate.
/// </summary>
public abstract class AbstractPluginPackageBuilder : PackageBuilder {

    static public string GetPluginName()
    {
        Path.
    }

    //[MenuItem("Digital Painting/Build/Build Rain Maker Package")]
    new public static void Build()
    {
        string pluginName = GetPluginName();
        string rootDir = "Assets/Digital Painting/Plugins/" + pluginName;
        string packageName = pluginName + ".unitypackage";

        AssetDatabase.ExportPackage(rootDir, packageName, ExportPackageOptions.Interactive | ExportPackageOptions.Recurse);
        Debug.Log("Exported " + packageName);
    }
}
