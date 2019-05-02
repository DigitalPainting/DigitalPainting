# Creating A Scene

The idea of this project is that you can use it as a base for your own Digital Paintings,
therefore you need to be able to add it to your own scene. This document describes how to
do that.

In this example we'll document using a scene purchased from the Asset Store, however you 
can use any scene that has a terrain withing it.

## Create a Base Project

Create your new project and create or import your scene. I've used the excellent [Nature 
Manufacture’s Meadow Environment – Dynamic Nature](https://assetstore.unity.com/packages/3d/vegetation/meadow-environment-dynamic-nature-132195)
asset. I'm using a copy of the `Unity Standard Demo Scene` that comes with the asset.
THe process should be the same if you want to use the gills demo scene, but that requires
Unity Pro and some other assets. 

The Meadow environment 
asset recommends you to setup some of the lighting and camera effects, but it should be
noted that some plugins for THe Digital Painting also advise specific lighting and camera 
effects. These plugins may overwrite you settings.

At the time of writing there is no way to force a specific setup. This is a feature that
needs to be added, see [issue #23](https://github.com/DigitalPainting/DigitalPainting/issues/23) 
to track this. If you hit a problem in this regard we would welcome your contribution.

There are a few steps you should take to ensure the best experience:

  1. If you want to use the Day Night Cycle feature of THe Digital Painting remove any lighting in the scene that will conflict with this. This is not necessary if you turn off or remove the Day Night Cycle in `Digital Painting Manager`.
  2. Import the post process stack 2.0 into your project (package manager--> post process stack)
  3. Change shadow distance to 500 or higher in quality settings

## Add the Core Digital Painting Assets

Having created a standard Unity scene you need to add The Digital Painting Assets. If you want to use
defaults this is pretty easy to do. You can either work from the source or from a released package:

### From Source

If you want to work from source then we recommend the use of [Git Submodules](https://git-scm.com/book/en/v2/Git-Tools-Submodules):

```
cd Assets
git submodule add git@github.com:DigitalPainting/DigitalPainting.git
git submodule add git@github.com:DigitalPainting/Flying-Pathfinding.git
git submodule add git@github.com:DigitalPainting/ScriptableObject-Architecture.git
```

You will likely get some compile errors. This is because you need to install some Unity packages using the Package Manager.
Pay close attention to the error messages, they tell you what you need to install.

### From Released Packages

  * [Export the Digital Painting asset](ReleasingTheDigitalPaintingAsset.md) from Unity (we should make releases so this is not necessary)
  * Import the Digital Painting asset into your project
  * If there are any compile errors import the required packages (see [README.md](../../../README.md) for details)
  * Add the `Assets/Digital Painting/Prefabs/DigitalPaintingManager`
    * Ensure that each of the features you want are enabled and configured properly (see docs folder)
    * Turn off the `Demo Manager` script if you don't wan the UI to overlay your scene

## Configure Your Scene to use the Digital Painting

  * Add the `DigitalPaintingManager` to your scene
  * Add the Octree prefab to the scene and ensure it covers your terrain
  * Add an [agent](agents.md) to the scene - the easiest way to get started is to add the Fairy agent that comes with the Digital Painting package
  * Ensure there is an `AgentWithFocus` reference variable set in the `Digital Painting Manager` and the `Director`

## DONE!

That's all you need to get started. Hit play and see what happens.

## Next Steps

  * Add [cinematic cameras](./Cameras.md)
  * Add a [Day Night Cycle](./DayNightCycle.md)
  * Add a [Weather System](./Weather.md)
  * Mark some [Interesting Things](./InterestingThings.md) for the camera to investigate
  





