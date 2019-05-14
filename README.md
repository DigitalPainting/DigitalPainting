# The Digital Painting

A Unity project that provides a "Digital Painting", a scene that is automatically explored by the camera. It provides a framework in which digital art can be created.

## Features of the Digital Painting

  * Automated Drone Exploration - see `Scripts/Agent/DroneController`
  * Designate items as [Interesting Things](./Assets/Digital%20Painting/Docs/InterestingThings.md) in the scene to be explored by the drone
  * Support for spawning items into the world using a [SimpleSpawner](./Assets/Digital%20Painting/Docs/SpawnableObjects.md)
  * [Pluggable Day Night Cycle](./Assets/Digital%20Painting/Docs/DayNightCycle.md)
    * Basic Day Night Cycle included
    * [Plugin](https://github.com/DigitalPainting/WeatherMakerPlugin) for [Digital Ruby's Weather Maker](https://assetstore.unity.com/packages/tools/particles-effects/weather-maker-sky-weather-water-volumetric-clouds-and-light-60955) 
  * [Pluggable Weather System](./Assets/Digital%20Painting/Docs/Weather.md)
    * [Plugin](https://github.com/DigitalPainting/RainMakerPlugin) for [Digital Ruby's Rain Maker (Free)](https://assetstore.unity.com/packages/vfx/particles/environment/rain-maker-2d-and-3d-rain-particle-system-for-unity-34938)
  * Pull current or forecasted weather ([powered by Dark Sky](https://darksky.net/poweredby/)) and reflect it in the Digital Painting
  * Easily add the Digital Painting asset to your own scene
    * Standard Assets example scene

## Getting Started

The Digital Painting asset comes with functional scenes that are designed for development and demonstration use. These scenes are very basic in their nature, using only primitives that comes with Unity by default. They are not designed to be pretty. There are some additional [demo scenes](./Assets/Digital%20Painting/Plugins/Scenes/README.md) included that are prettier, in some cases beautiful, though each of these has some additional dependencies that must be installed - some of which are not free. The [demo scenes](./Assets/Digital%20Painting/Plugins/Scenes/README.md) document details these.

To create your own Digital Painting scene you can start with any Unity scene that has a terrain. Simply add the `Digital Painting/Prefabs/DigitalPaintingManager` prefab to it. You are now ready to go, hit play.

This prefab will try to configure your scene to work with the Digital Painting. However, there are a few things you should be aware of, for example, if you plan to use the Day Night Cycle then your scene should not have any lighting that will interfere with the sun(s) provided by the cycle. 

For more details see
the [documentation on creating scenes](./Assets/Digital%20Painting/Docs/CreatingAScene.md).

### Required Packages (for use)

These packages will be imported for you when you install the Digital Painting asset. However, some of the them require manual steps to finalize the setup.

  * Cinemachine
    * No manual steps required
  * TextMesh Pro
    * Window -> TextMeshPro -> Import TMP Essential Resources

## Development

We use Git submodules to keep things clean and open the possibility of releasing some of the features as separate projects. Therefore in order to checkout the source you must use:

`git clone git@github.com:DigitalPainting/DigitalPainting.git test --recurse-submodules`

Be sure to import the packages listed above and the development assets in the next section. While these are free of cost they are not open source and so we cannot include them in the source.

In order to build and test features of the Digital Painting project all developers should create
or use the appropriate dev sene (see `Digital Patinting/scenes/dev`). No new features will be accepted without a
corresponding dev scene to allow testing of the feature. It is also important that the "Demo Scene" 
includes a demo of all new new features. 

The dev and demo scenes must use only a standard set of (zero cost) 
assets from the Unity Asset store and packages from Unity itself. The number of dependencies must be kept to a minimum.
Any features that are dependent on assets that are not free of cost must be optional and must degrade gracefully in this scene when 
only the assets and packages listed below are included. See the plugin mechanism for guidance on how to include features that
depend on paid assets.

### Required Assets and Packages for Development Work (all zero cost)

Download and import the following assets:

  * Unity Environment Standard Assets (Assets -> Import Package -> Environment)
    * You only need the `Environment` and `Effects/LightFlares`
    * You'll need to regenerate the SpeedTree assets (click the error then click `Apply & Generate Materials` in the inspector)
  * ProBuilder Package (install using Package Manager)
  * TextMesh Pro  (install using Package Manager)

### Credits

All textures included with this package are available from [opengameart.org](https://opengameart.org/textures/all) under a [CC0 license](https://creativecommons.org/share-your-work/public-domain/) and can therefore be reused without restriction. 

All icons provided in this asset are available from [openclipart.org](https://openclipart.org) under a public domain license and can therefore be used without restriction.

Please see the license details within git submodules. Only permissively licensed code is included. Changes to these submodules should be submitted upstream for inclusion in the originating project (at the owners disretion).







