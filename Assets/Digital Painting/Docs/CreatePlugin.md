The Digital Painting supports a number of third party assets as integrated plugins and, since it is open source, it is relateively easy for you to add your own plugins. The Plugin system provides a unified way to easily configure plugin assets, while the full asset is still accessible through the edito and extension code. This document describes how to implement a plugin and integrate it into the Editor UI.

For the purposes of this document we will explore the Day Night Plugins. The Digital Painting asset provides a Simple Day Night Plugin by default, you can also opt to use a paid implementation if you so desire. 

# Plugin Definition

Create script that defines the plugin you want to make available. This should implement the Abstract class `AbstractPluginDefinition` and is stored in the `Scritps/Plugin` folder of Digital Painting. This definition provides information useful in the UI, e.g. a human readable name, and information useful for enabling the plugin, e.g. the class that must be present in the Assembly for the plugin to be available.

For guidance on how to implement a plugin definition take a look at `SimpleDayNightPluginDefinition` which describes the built in Day Night cycle and the `WeatherMakerDayNightPluginDefinition` which describes a day night cycle provided as part of Digital Ruby's Weather Maker asset.

# Plugin Configuration

Plugins are designed to have a consistent configuration across all differing implementations. This configuration is stored in a Scriptable Object. The Digital Painting provides abstract classes that provides the common functionality across all implementations. The final profile class must be provided by the plugin implementation and can contain any customizations specific to that implementation of the plugin feature. For example, the Day Night plugins have their profiles in a class that implements the `AbstractDayNightProfile`. In the case of our built in `SimpleayNightPluginDefinition` we provide an example configuration file in `SimpleDayNightProfile`. The Scriptable Ovject is defined in the `Scripts` folder of the plugin directory.

```
namespace wizardscode.environment.weather
{
    [CreateAssetMenu(fileName = "WeatherMakerDayNightProfile", menuName = "Wizards Code/Day Night/Weather Maker")]
    public class WeatherMakerDayNightProfile : AbstractWeatherProfile
    {
      ...
    }
}
```

## Enablement and Validation code

Digital Painting tries to provide sensible defaults for all plugins, along with hints on how to configure things. The is done through one or more classes that implement `IValidationTest`. The Digital Painting provides core validation tests for each pluging type in `Scripts/Validation`. Plugins can provide their own additional tests. Each validation can provide an action that will help the user resolve any problems find. For more details see `Scripts/Validation/ValidateDayNightProfile`.

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

