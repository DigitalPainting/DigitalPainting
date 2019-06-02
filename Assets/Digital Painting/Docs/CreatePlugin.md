The Digital Painting supports a number of assets as integrated plugins and, since it is open source, it is relateively easy for you to add your own plugins. The Plugin system provides a unified way to easily configure the basic features of plugin assets of similar type, while the full asset is still accessible through the editor for power users. This document describes how to create a plugin and integrate it into the Editor UI.

To create a plugin you need to create a number of scripts, the following sections describe each. None of them are particularly complicated so the best way to learn is to look at examples provided by the Digital Painting asset. In this document we describe the creationg a Flying Camera Agent that can be used to explore the scene. You can find others to inspect in the [Digital Painting code repository](https://github.com/digitalpainting/).

TODO: push the camera agent plugin to GitHub

# Plugin Project

Plugins are built as separate Unity projects to be imported into your Digital Painting application. For convenience we use a naming convention to make it easy to identify different components in the Digital Painting setup. You are not forced to use these conventions but it is advisable.

Plugin projects are named `Plugin_CATEGORY_NAME`, where `Plugin` identifies this is a plugin project. `CATEGORY` is one of the values that describe what kind of plugin this is (listed below) and NAME is any name you want to use to uniquely identify this plugin.

Known categories (at the time of writing) are listed below. If there is no suitable category label already you can create your own. Please issue a pull request against this document and against the `PluginCategory` enum in `AbstractPluginDefinition`.

  * `Agent` - agents are things that move about in the world and interact with it.
  * `DayNightCycle` - these plugins control the day night cycle in the world
  * `Weather` - plugins to control the weather in the world
  * `Other` - plugins that do not fit neatly into any other category

We will call our example plugin `Plugin_Agent_ManualFlyingCamera`.

TODO: Rename existing plugins to conform to this naming convention.

## Plugin Manager

Each type of plugin must have an implementation of `AbstractPluginManager`, this is a `MonoBehavior` that manages the plugin implementation. When the plugin is installed into a scene, this is added as a component on a manager game object. The plugin managers role is to communicate between the Digital Painting engine and the plugin implementation, passing appropriate values back and forth. That is the plugin manager is the glue between the external asset and the Digital Painting asset.

The Digital Painting asset provides a number of these managers "out of the box", but they can also be provided by the plugin itself. In our example we will be using the `Agent_PluginManager`, you can find the source for this in `Scripts/Plugin/TYPE`, where type is as described in the naming convention above. 

Implementing the plugin manager is, usually, the most complicated part of building a plugin. However, it is nothing more than a standard MonoBehavior so anyone reasonably familiar with Unity should be able to implement on.

TODO: Rename all plugin manager implementations to match the naming convention of `Type_PluginManager`

## Plugin Definition

A plugin definition script describes the plugin to the Digital Painting core system. Create a new script following the naming convention of `Agent_ManualFlyingCamera_PluginDefinition` that implements the Abstract class `AbstractPluginDefinition` and is stored in the `Scripts` folder of your plugin project (you can put it anywhere in the scripts folder). 

Note that at this stage you won't have created some of the scripts you need in order to fully implement this class, don't worry, you will create them below.

This definition provides information useful in the UI, e.g. a human readable name, and information useful for enabling the plugin, e.g. the class that must be present in the Assembly for the plugin to be available. The `AbstractPluginDefinition` file should be well commented. Simply implement each of the methods and properties required. The existience of a plugin definition class, whether provided by the core Digital Painting asset or by an external plugin project will result in the Digital Painting Manager Window in the editor displaying athe plugin in the list of available plugins. The UI will also provide buttons to enable a plugin from each available group.

## Plugin Profile

Plugins are designed to have a consistent configuration across all differing implementations. This configuration is stored in a Profile Scriptable Object. To create a plugin profile create a file in your plugin scripts directory called `TYPE_DESCRIPTIVENAME_Profile`. You can have as many different profiles as you like, each will make the plugin behave in different ways. For our camera we will create `Agent_ManualFlyingCamera_PluginProfile`.

This class should provide a menu option for creating different profiles for the plugin:

```
[CreateAssetMenu(fileName = "Agent_ManuallyControlledFlyingCamer_Profile", menuName = "Wizards Code/Agent/Manually Controller Flying Camera")]
```

The plugin profile will provide 0 or more parameters defined by a ScriptableObject that implements the abstract generic class `GenericSettingSO<T>`. These parameters provide the code needed to configure the plugin and validate everything is setup correctly. Each setting will carry a suggested value that will be used to configure the plugin. They are used to provide feedback to the designer on how their Digital Painting should be configured and highlight places within which different plugins expect different settings.

For our plugin we will, for example, set the movement speed of our agent. To do this we will create a settings such as `NormalSpeedSettingSO`. For plugin types that the Digital Painting core asset are aware of you will find pre-existing implementations of these settings in the `ScriptableObjects/Validation/TYPE` folder. If there are settings that are unique to your plugin you can create them by creating classes that extend `GenericSettingSO`, see the next section for more details. 

Parameters are defined in the profile scriptable object as follows:

```
        [Expandable(isRequired: true)]
        [Tooltip("The speed the camera should move under normal circumstances.")]
        public NormalSpeedSettingSO normalSpeed;
```

Note the use of the \[Expandable\] attribute here. This is a useful attribute that does two things. Firstly, it makes the attribute in the Inspecter expadandable, when expanded it will display the editor for this attribute type. Secondly it optionally marks the attribute as required. If `isRequired` is set to true then Digital Painting will display an error in the inspector if no value is provided.

### Creating new Setting Scriptable Objects

Settings are defined by defined by a ScriptableObject that implements the abstract generic class `GenericSettingSO<T>`. At a minimum this required you to implement the `TestName` and `ActualValue` methods:

`TestName`: returns a human readable name for the setting that will be used in the UI, we will use `Normal Speed`
`ActualValue`: a getter and setter for the value that is set in Unity

Many settings will also need to overide:

`Fix`: a default method used to fix the plugin setup to use the suggested value provided in the profile. The default implementation of this will simply set the desired value by using the `ActualValue` setter and the provided `SuggestedValue` in the profile.

`ValidateSetting(Type validationTest)`: This checks to see if the value is correctly set. If not it will report an error or warning in the editor and (optionally) offer a button to fix the issue. The default implementation will work in cases where we only need to check that a setting matches the selected value (using the `ActualValue` getter) or, if the suggested value is a prefab, this value will check whether it has been instantiated in the scene (when required). For more complex validation steps override this method.

## ValidationTest<T> implementation

Through the plugin profile the Digital Painting core code attempts to ensure everything is setup correctly. The profile defines what correctly looks like while a class that extends the `ValidationTest<T>` class is used to perform the necessary tests. The goal is to make it as easy as possible to setup key assets to get started, without preventing the user from dropping into the advanced configuration offered by the assets themselves.

In order to trigger this validation (and make the plugin available in the editor window) we ned to extend `ValidationTest<T>` where T is an implementation of `AbstractPluginManager` as `TYPE_DESCRIPTIVENAME_PluginValidation`. In our agent example we will create `Agent_ManualFlyingCamera_PluginValidation` which implements `ValidationTest<Agent_PluginManager>` in our plugin project. This requires us to implement a small number of methods:

`Instance`: this simply needs to return an instance of the class, in our example case `new Agent_ManualFlyingCamera_PluginValidation();`
`ProfileType`:  this returns the name of the profile type we created earlier, in the example case `"Agent_ManualFlyingCamera_PluginProfile"`

At this point you should see your plugin in the editor window and you can use the tooling to add it and to configure it as described below.

# Testing your plugin

Once all this is done it's really easy for users to use the plugin. In a test scene work through the followin process to test the plugin.

  1. If the plugin has a dependency on another asset and that button is not present in the project there will be a button to take you to the location where you can obtain the asset. This location is defined in the plugin profile `GetURL()` method.
  2. Click the button to enable the plugin in your scene, by default this will add the plugin manager to the `Wizards Code/Digital Painting Manager`, if your plugin requires different behaviour then override the `Enable()` method in the plugin definition.
  3. If you already have a suitable plugin profile defined assign that to the Plugin Manager created in the previous step, if no such profile exists then create one using `Assets -> Create -> Wizards Code -> TYPE -> DESCRIPTIVENAME Plugin Profile` (if the option is not available it is because you forgot to add `[CreateAssetMenu(...")])` to the Plugin Profile script (see above).
  4. Drag the chosen profile into the `Plugin Profile` slot of the plugin manager created in step 2
  5. Resolve any issues reported in the Digital Painting editor window (for user convenience there is an editor presented in the plugin manager component, see below for more on how to do this)
  6. Play the Scene

FIXME: make step 4 automatic... select a default profile if one is available or offer to create a new one in a location of convention 

# Creating Setting Scriptable Object instances

It can be a little laborious to create the settings initially. Rather than simply setting a value we need to create a Scriptable Object that will hold this value. However, it is worth the effort, as these values can be shared across many different implementations of the plugin and game objects provided by the plugin. This means that we only need to edit the value once for it to take effect across the whole Digital Painting. To create an SO use `Assets -> Create -> Wizards Code -> Validation -> TYPE -> SETTING_TYPE`.
