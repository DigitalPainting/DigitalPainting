# The Digital Painting

A Unity project that provides a "Digital Painting", a scene that is automatically explored by the camera. It provides a framework in which digital art can be created.

## Features of the Digital Painting

  * Automated Drone Navigation - see `Scripts/Agent/DroneController`
  * [Mark Interesting Things](./Assets/Digital%20Painting/Docs/InterestingThings.md) in the scene to be explored by the drone
  * Pluggable Day Night Cycle support
    * Basic Day Night Cycle included
  * Easily add the Digital Painting asset to your own scene 

## Getting Started

To create your own Digital Painting scene you should start with the `Starter Template`. Open it 
and save it as a new scene. Then add your terrain and objects to it. Alternatively,
move the objects in the starter scene into your existing scene. For more details see
the [documentation on creating scenes](./Assets/Digital%20Painting/Docs/InterestingThings.md).

### Required Packages (for development and use)

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

### Required Assets for Development Work (all zero cost)

Download and import the following assets:

  * Unity Environment Assets (Assets -> Import Package -> Environment)
    * You only need the `Environment` and `Effects/LightFlares`
    * You'll need to regenerate the SpeedTree assets (click the error then click `Apply & Generate Materials` in the inspector)
  * [Fantasy Landscape](https://assetstore.unity.com/packages/3d/environments/fantasy-landscape-103573)









