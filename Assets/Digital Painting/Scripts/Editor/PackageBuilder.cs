﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Build the package for release. Grabs all the available plugin packages
/// and puts them in the `plugins` folder then builds the Unity Package
/// for The Digital Painting.
/// 
/// Run from Unity with `Digital Painting -> Build -> Build Core Package`
/// 
/// Run from the command line with:
///   `"C:\Program Files\Unity\Editor\Unity.exe" -executeMethod PackageBuilder.Build`
/// </summary>
public class PackageBuilder
{

    [MenuItem("Tools/Build Package/Digital Painting/Core")]
    public static void Build()
    {
        string[] rootDirs = { "Assets\\Digital Painting", "Assets\\Flying-Pathfinding", "Assets\\Gizmos\\DigitalPainting", "Assets\\ScriptableObject-Architecture", "Assets\\SO Architecture" };
        string excludeSubDir = "Plugins";
        string packageName = @"..\DigitalPainting.unitypackage";

        // Collect available plugins
        GetPlugins();

        // Delete everything in plugins directory except *.unitypackage and *.md (and matching .meta)
        foreach (string rootDir in rootDirs) {
            MoveExcludedFiles(rootDir + "\\" + excludeSubDir);
        }
        AssetDatabase.Refresh();

        AssetDatabase.ExportPackage(rootDirs, packageName, ExportPackageOptions.Interactive | ExportPackageOptions.Recurse);
        Debug.Log("Exported " + packageName);

        foreach (string rootDir in rootDirs)
        {
            RecoverExcludedFiles(rootDir + "\\" + excludeSubDir);
        }
        AssetDatabase.Refresh();
    }

    protected static void MoveExcludedFiles(string dir)
    {
        if (File.Exists(dir))
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
                    Debug.Log("Temporarily moved " + subdirectory);
                }
                else
                {
                    MoveExcludedFiles(subdirectory);
                }
            }
        }
    }

    protected static void RecoverExcludedFiles(string dir)
    {
        string copyPath = "Temp" + Path.DirectorySeparatorChar + dir;
        if (File.Exists(copyPath))
        {
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
                    Debug.Log("Moved back " + targetPath);
                }
                else
                {
                    RecoverExcludedFiles(targetPath);
                }
            }
            Directory.Delete(copyPath, true);
        }
    }

    protected static void GetPlugins()
    {
        string[] files =  Directory.GetFiles("..", "*.unitypackage");
        foreach (string file in files)
        {
            if (file != "..\\DigitalPainting.unitypackage") {
                string targetPath = "Assets\\Digital Painting\\Plugins\\" + file.Substring(file.IndexOf(Path.DirectorySeparatorChar, 1) + 1);
                if (File.Exists(targetPath))
                {
                    File.Delete(targetPath);
                }
                File.Move(file, targetPath);
                Debug.Log("Imported plugin package " + file + " to " + targetPath);
            }
        }
    }

}
