using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Build the package for release.
/// 
/// Run from Unity with `Digital Painting -> Build -> Build Core Package`
/// 
/// Run from the command line with:
///   `"C:\Program Files\Unity\Editor\Unity.exe" -executeMethod PackageBuilder.Build`
/// </summary>
public class PackageBuilder {

    [MenuItem("Digital Painting/Build/Build Core Package")]
    public static void Build()
    {
        string rootDir = "Assets/Digital Painting";
        string excludeSubDir = "Plugins";
        string packageName = "DigitalPainting.unitypackage";

        // Delete everything in plugins directory except *.unitypackage and *.md (and matching .meta)
        RemoveDirs(rootDir + "/" + excludeSubDir);
        AssetDatabase.Refresh();

        AssetDatabase.ExportPackage(rootDir, packageName, ExportPackageOptions.Interactive | ExportPackageOptions.Recurse);
        Debug.Log("Exported " + packageName);
    }

    private static void RemoveDirs(string dir)
    {
        string[] subdirectoryEntries = Directory.GetDirectories(dir);
        foreach (string subdirectory in subdirectoryEntries)
        {
            if (Path.GetFileName(subdirectory) != "Scenes")
            {
                Debug.Log("Deleting " + subdirectory + " and associated `.meta` file.");
                Directory.Delete(subdirectory, true);
                File.Delete(subdirectory + ".meta");
            }
            else
            {
                RemoveDirs(subdirectory);
            }
        }
    }

}
