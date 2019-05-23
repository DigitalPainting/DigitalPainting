The Digital Painting supports a number of third party assets as integrated plugins and, since it is open source, it is relateively easy for you to add your own plugins. The Plugin system provides a unified way to easily configure plugin assets, while the full asset is still accessible through the edito and extension code. This document describes how to implement a plugin and integrate it into the Editor UI.

For the purposes of this document we will explore the Day Night Plugins. The Digital Painting asset provides a Simple Day Night Plugin by default, you can also opt to use a paid implementation if you so desire. 

# Plugin Definition

Within the Digital Painting Asset Plugins must have a class that describes how to find and integrate with the plugin. These definitions are stored in `Scritps/Plugin` and should implement the Abstract class `AbstractPluginDefinition`. Since there will often be multiple possible implementations type of plugin it often makes sense to create a second abstract class that extends `AbstractPluginDefinition`, for example see `AbstractDayNightPluginDefinition`. Specific plugin definitions will then implement this class. 

The definition provides information useful in the UI, e.g. a human readable name, and information useful for enabling the plugin, e.g. the class that must be present in the Assembly for the plugin to be available.

pFor guidance on how to implement a plugin definition take a look at `BasicDayNightPluginDefinition` which describes the built in Day Night cycle and the `WeatherMakerDayNightPluginDefinition` which describes a day night cycle provided as part of Digital Ruby's Weather Maker asset.

# Plugin Configuration

Plugins are designed to have a consistent configuration across all differing implementations. This configuration is stored in a Scriptable Object. For example, the Day Night plugins have their profiles in a class that implements the `AbstractDayNightProfile`. In the case of our built in `BasicDayNightPluginDefinition` we provide an example configuration file in `SimpleDayNightProfile`. 

# Plugin Implementation

The majority of the implementation code for a plugin will come from an external asset. However, we do need some glue code to translate the base Digital Painting configuration to the assets expected format. In some cases we will also need some code to interface between the two systems. This code will be provided in the same class as the configuration discussed in the previous section. For example, for our Day Night plugin the setup code is in the implementation of `AbstractDayNightProfile`.

## Enable

FIXME: Install the plugin package
FIXME: run the Enable script in the Plugin Definition - adds the manager component, does required config
FIXME: add the configuration to the manager

# Using a Plugin

Once all this is done it's really easy for users to use the plugin.

  1. Click the button to view the asset in the store
  2. Purchase and install the asset
  3. Click the button to enable the plugin in your scene
  3.5. Add the WeatherMakerScript to your DigitalPaintingManager
  4. Create a day night profile using `Assets -> Create -> Wizards Code/Weather Maker Day Night Profile`
  5. Duplicate a Weather Profile provided by Weather Maker
  5. Drag the Weather profile into the `Weather Maker Profile` slot of the plugin profile
  6. Drag the plugin profile into the `Day Night Cycle Manager` on the `Digital Painting Manager`
  6. Play the Scene

FIXME: make step 3.5 automatic... provide a prefab and make it a child of the Digital Painting Manager?
FIXME: make step 4 automatic... select an existing profile or offer to create a new one in a known location - `profiles/plugins`
FIXME: make setp 5 automatic... select an existing profile, duplicate it into `profiles/weathermaker`
FIXME: Provide an editor for the plugin profile and expose it in the plugin manager

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