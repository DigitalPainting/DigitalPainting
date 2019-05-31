The Digital Painting supports a number of third party assets as integrated plugins and, since it is open source, it is relateively easy for you to add your own plugins. The Plugin system provides a unified way to easily configure plugin assets, while the full asset is still accessible through the editor and extension code. This document describes how to implement a plugin and integrate it into the Editor UI.

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

The plugin profile will provide 1 or more parameters defined by a ScriptableObject that implements `GenericSettingSO`. There are a number of these Scriptable Objects defined in `ScriptableObjects/Validation` (see next section for more information on these settings objects). For exammple:

```
        [Header("Environment settings")]
        [Expandable(isRequired: true, isRequiredMessage: "Must provide a suggested skybox setting.")]
        public SkyBoxSettingsSO Skybox;
        
        [Expandable(isRequired: true, isRequiredMessage: "Must provide a suggested Sun setting.")]
        [Tooltip("A prefab containing the directional light that acts as the sun. If blank a light with the name `Sun` will be used.")]
        public SunSettingsSO SunPrefab;
```

Note the use of the \[Expandable\] attribute here. This is a useful attribute that does two things. Firstly, it makes the attribute in the Inspecter expadandable, when expanded it will display the editor for this attribute type. Secondly it optionally marks the attribute as required. If `isRequired` is set to true then Digital Painting will display an error in the inspector if no value is provided.

## Settings Scriptable Objects

The classes that extend the `GenericSettingSO` provide the code needed to validate a setting and the plugin configuration. Each setting will carry a suggested value that will be used to configure the plugin. In the above example we see that we have two settings objects, one for the Skybox and one for the Sun prefab. The implementations of these classes simply implement the `TestName`, `ActualValue` and `Fix` methods. This is the minimum needed to create an Settings object, the role of these methods is:

`TestName`: returns a human readable name for the setting that will be used in the UI
`ActualValue`: a getter and setter for the value that is set in Unity
`Fix`: a default method used to fix the plugin setup to use the suggested values.

Some plugins will also want to override the `ValidateSetting(Type validationTest)` method. The default solution provided will work in cases where we only need to check that a setting matches the selected value and if the suggested value is a prefab, it has been instantiated in the scene (when required). For more complex validation steps override this method.

# Plugin Manager

Each plugin should provide an implementation of `AbstractPluginManager`, this is a `MonoBehavior` that manages the plugin implementation. This is added to a game object in the scene communicates between the Digital Painting engine and the plugin, passing appropriate values back and forth. That is the plugin manager is the glue between the external asset and the Digital Painting asset.

Users must provide a plugin profile (see above) to the plugin manager.

The existinence of a plugin manager class, whether provided by the core Digital Painting asset or by some plugin enablement code from a third party, will result in the Digital Painting Manager Window in the editor displaying a list of available plugins, grouped by type. The UI will also provide buttons to enable a plugin from each available group.

## Validating the plugin setup

Digital Painting tries to provide sensible defaults for all plugin types, along with hints on how to configure things. The goal is to make it as easy as possible to setup key assets to get started, without preventing the user from dropping into the advanced configuration offered by the assets themselves.

This is done through one or more classes that implement `ValidationTest<T>` where T is an implementation of AbstractPluginManager. When an instance of a plugin manager is found in the scene the `Validate` method of the `ValidationTest` class is called. The `ValidationTest` manages the execution of the `Validate` method in the implementation of the `GenericSettingSO` object (see above).

The results of these tests are presented in the Digital Painting Manager Window in the Editor and will often provide helpers that can be assigned to buttons that, when clicked, attempt to resolve problems found. Users will also be able to mark a test as ignored. This allows them to create setups that go beyond the default settings provided by the Digital Painting asset.

# Using a Plugin

Once all this is done it's really easy for users to use the plugin.

  1. Click the button to view the asset in the store
  2. Purchase and install the asset
  3. Click the button to enable the plugin in your scene
  4. Create a day night profile using `Assets -> Create -> Wizards Code/Weather Maker Day Night Profile`
  5. Duplicate a Weather Profile provided by Weather Maker
  5. Drag the Weather profile into the `Weather Maker Profile` slot of the plugin profile
  6. Drag the plugin profile into the `Day Night Cycle Manager` on the `Digital Painting Manager`
  6. Play the Scene

FIXME: make step 4 automatic... select an existing profile or offer to create a new one in a known location - `profiles/plugins`
FIXME: make setp 5 automatic... select an existing profile, duplicate it into `profiles/weathermaker`
FIXME: Provide an editor for the plugin profile and expose it in the plugin manager

