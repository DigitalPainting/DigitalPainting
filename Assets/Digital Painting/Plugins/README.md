# Plugins

In this folder you will find a number of plugins that add functionality to The Digital Painting. In many cases these will depend on third party assets that must be purchased separately.

## Installation and Setup


  1. Install dependencies (see descriptions below)
  2. Install the plugin just like any other unity package - simply double click the package file. 
  3. Create a configuration file using `Assets -> Create -> Wizards Code/PLUGIN TYPE ... ` There will be entries in this folder for each of the plugins you have installed
  4. Drag the generated configuration file into the `Configuration` field of the `PLUGIN Manager` component attached to the `DigitalPaintingManager` game object
  5. Double click the configuration file and configure as appropriate
  6. Check the README.md in the root of the plugin directory for more information about each specific plugin. It will be called 'Assets/DigitalPainting/Plugin/PLUGINTYPE_PLUGINNAME'
  7. Most plugins will have at least a `Scenes/DevTest/DevTest` scene, but they may include other demo scenes

Be aware that any dependencies must be installed first if you want to avoid build errors. Don't worry though, you can install the dependencies afterwards and the errors should be resolved.

## Available Plugins

Below you will find a list of available plugins along with an indication of any dependency they have. Note that some plugins fit into multiple categories and are thus listed multiple times.

### Day Night Cycle

These plugins provide a day night cycle that changes the lighting according to the time of day in the scene. The Digital Painting comes with a very basic Day/Night Cycle, the plugins here will provide a much better lucking experience.

#### Digital Ruby's Weather Maker

[Digital Ruby's Weather Maker](https://assetstore.unity.com/packages/tools/particles-effects/weather-maker-sky-weather-water-volumetric-light-60955) includes an advanced Day Night cycle. Please purchase and install this asset first.

### Weather 

The following plugins add weather systems to The Digital Painting.

#### Digital Ruby's Rain Maker (Free)

### Scenes

#### Standard Assets Only (Free)

This is a simple demo scene using the Unity Standard Assets.

You must install the Environment elements from the Unity Standard Assets.



