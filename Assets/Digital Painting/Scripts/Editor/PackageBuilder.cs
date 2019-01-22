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
public class PackageBuilder
{

    [MenuItem("Digital Painting/Build/Build Core Package")]
    public static void Build()
    {
        string rootDir = "Assets\\Digital Painting";
        string excludeSubDir = "Plugins";
        string packageName = @"..\DigitalPainting.unitypackage";

        // Delete everything in plugins directory except *.unitypackage and *.md (and matching .meta)
        MoveExcludedFiles(rootDir + "\\" + excludeSubDir);
        AssetDatabase.Refresh();

        AssetDatabase.ExportPackage(rootDir, packageName, ExportPackageOptions.Interactive | ExportPackageOptions.Recurse);
        Debug.Log("Exported " + packageName);

        RecoverExcludedFiles(rootDir + "\\" + excludeSubDir);
        AssetDatabase.Refresh();
    }

    protected static void MoveExcludedFiles(string dir)
    {
        string[] subdirectoryEntries = Directory.GetDirectories(dir);
        foreach (string subdirectory in subdirectoryEntries)
        {
            if (Path.GetFileName(subdirectory) != "Scenes")
            {
                Debug.Log("Moving to safety: " + subdirectory);
                string copyPath = "Temp" + Path.DirectorySeparatorChar + subdirectory;
                Directory.CreateDirectory(Path.GetDirectoryName(copyPath));
                Directory.Move(subdirectory, copyPath);
                File.Move(subdirectory + ".meta", copyPath + ".meta");
            }
            else
            {
                MoveExcludedFiles(subdirectory);
            }
        }
    }

    protected static void RecoverExcludedFiles(string dir)
    {
        string copyPath = "Temp" + Path.DirectorySeparatorChar + dir;
        string[] subdirectoryEntries = Directory.GetDirectories(copyPath);
        foreach (string subdirectory in subdirectoryEntries)
        {
            string targetPath = subdirectory.Substring(subdirectory.IndexOf(Path.DirectorySeparatorChar, 1) + 1);
            if (Path.GetFileName(subdirectory) != "Scenes")
            {
                Debug.Log("Moving back to project from: " + subdirectory + " to " + targetPath);
                Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
                Directory.Move(subdirectory, targetPath);
                File.Move(subdirectory + ".meta", targetPath + ".meta");
            }
            else
            {
                RecoverExcludedFiles(targetPath);
            }
        }
        Directory.Delete(copyPath, true);
    }

}
