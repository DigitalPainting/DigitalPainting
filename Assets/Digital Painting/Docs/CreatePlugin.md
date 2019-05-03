# Building a Plugin or Scene

The Digital Painting asset can be extended using plugins, for example, you can add support for your favorite Weather asset. It's also possible to package Scenes using this same mechanism. Plugins and Scenes can be built to use paid for assets without redistributing those assets. Of course users will need to install those assets before using the plugin or scene. These plugins and scenes are distributed as Unity Packages in the `Plugins` folder.

To create a new plugin you need to consult the documentation for that kind of plugin, for example [Weather](Weather.md), [Day Night Cycle](DayNightCycle.md) or [Scene](CreatingAScene.md).

In order to build a Unity Package for your plugin you should provide a menu command. The Digital Painting provides a easy way to do this. Basically create a script in your plugins `Scripts/Editor` folder called `PULUINNAMEPackageBuild`. This script should extend the `PackageBuilder` class and provide a new implementation of the static `Build` method. This method will use the `MoveExcludedFiles` method to move everything that should not be packaged out of the way, then it will Build the package, finally it will use the `RecoverExcludedFiles` method to restore the previous state.

Be sure to update the `MenuItem` line to create a new Build menu item.

```
using UnityEditor;
using UnityEngine;

public class RainMakerPackageBuilder : PackageBuilder {
    [MenuItem("Digital Painting/Build/Build RainMaker Plugin")]
    new public static void Build()
    {
        string rootDir = "Assets\\Digital Painting\\Plugins\\Weather_RainMaker";
        string packageName = "Weather_RainMaker.unitypackage";

        MoveExcludedFiles(rootDir);

        AssetDatabase.ExportPackage(rootDir, packageName, ExportPackageOptions.Interactive | ExportPackageOptions.Recurse);
        Debug.Log("Exported " + packageName);

        RecoverExcludedFiles(rootDir);
    }
}
```