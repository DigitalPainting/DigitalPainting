# The Digital Painting

A Unity project that provides a "Digital Painting", a scene that is automatically explored by the camera. It provides a framework in which digital art can be created.

## Features of the Digital Painting

  * Automated Drone Navigation - see `Scripts/Agent/DroneController`
  * [Mark Interesting Things](./Assets/Digital%20Painting/Docs/InterestingThings.md) in the scene to be explored by the drone
  * Pluggable Day Night Cycle support
    * Basic Day Night Cycle included
  * Easily add the Digital Painting asset to your own scene 

## Getting Started

To create your own Digital Painting scene you can start with any Unity scene that has a terrain. Simply add the `Digital Painting/Prefabs/DigitalPaintingManager` prefab to it. You are now ready to go, hit play.

This prefab will try to configure your scene to work with the Digital Painting. However, there are a few things you should be aware of, for example, if you plan to use the Day Night Cycle then your scene should not have any lighting that will intefere with the sun(s) provided by the cycle. 

For more details see
the [documentation on creating scenes](./Assets/Digital%20Painting/Docs/InterestingThings.md).

### Required Packages (for use)

Install these packages using Window -> Package Manager in Unity.

  * Cinemachine

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









