# Creating A Scene

The idea of this project is that you can use it as a base for your own Digital Paintings,
therefore you need to be able to add it to your own scene. This document describes how to
do that.

In this example we'll document using a scene purchased from the Asset Store, however you 
can use any scene that has a terrain withing it.

### Create a Base Project

Create your new project and create or import your scene. I've used the excellent [Nature 
Manufacture’s Meadow Environment – Dynamic Nature](https://assetstore.unity.com/packages/3d/vegetation/meadow-environment-dynamic-nature-132195)
asset. I'm using a copy of the `Unity Standard Demo Scene` that comes with the asset.
THe process should be the same if you want to use the gills demo scene, but that requires
Unity Pro and some other assets. 

Note, that your scene must have a terrain within it. This is used for agent navigation.

The Meadow environment 
asset recommends you to setup some of the lighting and camera effects, but it should be
noted that some plugins for THe Digital Painting also advise specific lighting and camera 
effects. These plugins may overwrite you settings.

At the time of writing there is no way to force a specific setup. This is a feature that
needs to be added, see [issue #23](https://github.com/DigitalPainting/DigitalPainting/issues/23) 
to track this. If you hit a problem in this regard we would welcome your contribution.

There are a few steps you should take to ensure the best experience:

  1. If you want to use the Day Night Cycle feature of The Digital Painting remove any lighting in the scene that will conflict with this. This is not necessary if you turn off or remove the Day Night Cycle in `Digital Painting Manager`.
  2. Import the post process stack 2.0 into your project (package manager--> post process stack)
  3. Change shadow distance to 500 or higher in quality settings

### Add the Core Digital Painting Assets

Having created a standard Unity scene you need to add The Digital Painting Assets. If you want to use
defaults this is pretty easy to do. Firstly configure the pacakges to be installed by adding the following lines to your `Packages/manifest.json` file:

```
    "com.danieleverland.scriptableobjectarchitecture": "https://github.com/DanielEverland/ScriptableObject-Architecture.git#release/stable",
    "com.unity.cinemachine": "2.2.8",
```

FIXME: When we have made releases of the other assets we use install those via the package manager. For now see the 'From Source' section below.

Next you need to insert the Digital Paintin code. You can either work from the source or from a released package:

### From Source

If you want to work from source then we recommend the use of [Git Submodules](https://git-scm.com/book/en/v2/Git-Tools-Submodules):

```
cd Assets
git submodule add git@github.com:DigitalPainting/DigitalPainting.git
git submodule add git@github.com:DigitalPainting/Flying-Pathfinding.git
```

Note there will be some warnings when the code is imported. This is caused by a limitation of the way Unity imports packages that are git submodules. So far, apart from the annoying warnings on import and startup we've not found any problems.

## Configure Your Scene to use the Digital Painting

  * Add the `DigitalPaintingManager` prefab to your scene
  * Add the Octree prefab to the scene and ensure the Box Collider covers your terrain (for a standard 500 x 500 terrain this means you should position the octree at 250, 0, 250 and set the box collider scale to 500, 100, 500 - note that the y value changes how high your flying agents can go)
  * Add an agent to the scene - the easiest way to get started is to add the Fairy agent that comes with the Digital Painting package, see the 'Spawning Agents' section of the [agent](agents.md) documentation.

## DONE!

That's all you need to get started. Hit play and see what happens.

## Next Steps

  * Add [cinematic cameras](./Cameras.md)
  * Add a [Day Night Cycle](./DayNightCycle.md)
  * Add a [Weather System](./Weather.md)
  * Mark some [Interesting Things](./InterestingThings.md) for the camera to investigate
  





