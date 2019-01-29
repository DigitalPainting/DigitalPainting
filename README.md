# The Digital Painting

A Unity project that provides a "Digital Painting", a scene that is automatically explored by the camera. It provides a framework in which digital art can be created.

## Features of the Digital Painting

  * Automated Drone Exploration - see `Scripts/Agent/DroneController`
  * Designate items as [Interesting Things](./Assets/Digital%20Painting/Docs/InterestingThings.md) in the scene to be explored by the drone
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
the [documentation on creating scenes](./Assets/Digital%20Painting/Docs/InterestingThings.md).

### Required Packages (for use)

These packages will be imported for you when you install the Digital Painting asset. However, some of the them require manual steps to finalize the setup.

  * Cinemachine
    * No manual steps required
  * TextMesh Pro
    * Window -> TextMeshPro -> Import TMP Essential Resources

## Development

All the assets for the Digital Painting project are in the `Digital Painting` folder. Be 
sure to import the packages listed above and the development assets below. There is a 
`Scenes/Dev Scene` which is intended to be used to build and test new features.

In order to build and test features of the Digital Painting project all developers use 
the "Dev Scene" to develop new features. This scene uses a standard set of (zero cost) 
assets from the Unity Asset store and packages from Unity itself. Any features that are 
dependent on other assets must be optional and must degrade gracefully in this scene when 
only the assets and packages listed below are included.

### Required Assets and Packages for Development Work (all zero cost)

Download and import the following assets:

  * Unity Environment Standard Assets (Assets -> Import Package -> Environment)
    * You only need the `Environment` and `Effects/LightFlares`
    * You'll need to regenerate the SpeedTree assets (click the error then click `Apply & Generate Materials` in the inspector)
  * ProBuilder Package (install using Package Manager)
  * TextMesh Pro  (install using Package Manager)

### Credits

All textures included with this package are available from [opengameart.org](https://opengameart.org/textures/all) under either a [CC0 license](https://creativecommons.org/share-your-work/public-domain/) and can therefore be reused without restriction. 

All icons provided in this asset are available from [openclipart.org](https://openclipart.org) under a public domain license and can therefore be used without restriction.

Flying Pathfinding is under and MIT license and was originally pulled from [Simeonradivoev](https://github.com/simeonradivoev/Flying-Pathfinding). I've made some changes and am pushing them upstream, but there is a chance that they won't be included in the upstream project. My [fork](https://github.com/rgardler/Flying-Pathfinding) is available and contains a standalone version with all the changes found in the digital painting repository.








