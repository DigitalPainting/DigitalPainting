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
The process should be the same if you want to use the gills demo scene, but that requires
Unity Pro and some other assets. 

Note, that your scene must have a terrain within it. This is used for agent navigation.

FIXME: Validate the scene has a terrain when the DigitalPaintingManager is added

The Meadow environment 
asset recommends you to setup some of the lighting and camera effects, but it should be
noted that some plugins for The Digital Painting also advise specific lighting and camera 
effects. These plugins may overwrite your settings.

At the time of writing there is no way to force a specific setup. This is a feature that
needs to be added, see [issue #23](https://github.com/DigitalPainting/DigitalPainting/issues/23) 
to track this. If you hit a problem in this regard we would welcome your contribution.

### Add the Core Digital Painting Assets

Having created a standard Unity scene you need to add The Digital Painting Assets. If you want to use
defaults this is pretty easy to do. Firstly configure the pacakges to be installed by adding the following lines to your `Packages/manifest.json` file:

```
    "com.danieleverland.scriptableobjectarchitecture": "https://github.com/DanielEverland/ScriptableObject-Architecture.git#release/stable",
    "com.unity.cinemachine": "2.2.8",
```

Next you need to insert the Digital Painting code.

FIXME: When we have made releases of the other assets we use install those via the package manager. For now see the 'From Source' section below.

### From Source

If you want to work from source then we recommend the use of [Git Submodules](https://git-scm.com/book/en/v2/Git-Tools-Submodules):

```
cd Assets
git submodule add git@github.com:DigitalPainting/DigitalPainting.git
git submodule add git@github.com:DigitalPainting/Flying-Pathfinding.git
```

Note there may be some warnings when the code is imported. This is caused by a limitation of the way Unity imports packages that are git submodules. So far, apart from the annoying warnings on import and startup we've not found any problems.

## Configure Your Scene to use the Digital Painting

  * `Window > Wizards Code > Open Digital Painting Manager`
  * Select the `Standard` tab and clidk `Add Digital Painting Manager`
  * Add an agent to the scene - the easiest way to get started is to add the Fairy agent that comes with the Digital Painting package, see the 'Spawning Agents' section of the [agent](agents.md) documentation.

## DONE!

That's all you need to get started. Hit play and see what happens.

## Next Steps

  * Add [cinematic cameras](./Cameras.md)
  * Add a [Day Night Cycle](./DayNightCycle.md)
  * Add a [Weather System](./Weather.md)
  * Mark some [Interesting Things](./InterestingThings.md) for the camera to investigate
  





