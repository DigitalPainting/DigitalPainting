The Digital Painting supports a number of third party assets as integrated plugins. It provides a unified way to easily configure these plugins while the full asset is still accessible through code. This document describes how to implement a plugin and integrate it into the Editor UI.

# Plugin Discovery

Plugins that are to be activated must have a class that definintion in the main Digital Painting asset that describes how to find and integrate with the plugin. These are stored in `Scritps/Plugin`. These classes should implement the Abstract class `AbstractPluginDefinition`. Since there will often be multiple possible implementations for each type of class it often makes sense to create a second abstract class that extends `AbstractPluginDefinition`, for example see `AbstractDayNightPluginDefinition`, specific plugin definitions will then implement this class. 

For guidance on how to implement a plugin definition take a look at `BasicDayNightPluginDefition` which describes the built in Day Night cycle and the `WeatherMakerDayNightPluginDefinition` which describes a day night cycle provided as part of Digital Ruby's Weather Maker asset.pushd

# LEGACY: Building a Plugin or Scene

FIXME: replace the custom build system with a Package Manager system

The Digital Painting asset can be extended using plugins, for example, you can add support for your favorite Weather asset. It's also possible to package Scenes using this same mechanism. Plugins and Scenes can be built to use paid for assets without redistributing those assets. Of course users will need to install those assets before using the plugin or scene. These plugins and scenes are distributed as Unity Packages in the `Plugins` folder.

To create a new plugin you need to consult the documentation for that kind of plugin, for example [Weather](Weather.md), [Day Night Cycle](DayNightCycle.md) or [Scene](CreatingAScene.md).

In order to build a Unity Package for your plugin you should provide a menu command. The Digital Painting provides a easy way to do this. Basically create a script in your plugins `Scripts/Editor` folder called `PLUGINNAMEPackageBuild`. This script should extend the `PackageBuilder` class and provide a new implementation of the static `Build` method. This method will use the `MoveExcludedFiles` method to move everything that should not be packaged out of the way, then it will Build the package, finally it will use the `RecoverExcludedFiles` method to restore the previous state.

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