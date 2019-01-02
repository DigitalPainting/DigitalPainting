# Creating A Scene

The idea of this project is that you can use it as a base for your own Digital Paintings,
therefore you need to be able to add it to your own scene. This document describes how to
do that.

In this example we'll document using a scene purchased from the Asset Store, however you can use any scene that has a terrain withing it.

## Create a Base Project

Create your new project and create or import your scene. I've used the excellent Nature 
[Manufacture’s Meadow Environment – Dynamic Nature](https://assetstore.unity.com/packages/3d/vegetation/meadow-environment-dynamic-nature-132195)
asset. I'm using a copy of the demo scene that comes with the asset. This particular
asset recommends you to setup some of the lighting and camera effects, so I did that.

## Add the Digital Painting Assets

For now this is a manual process, but someone could submit a patch to automate it (please).

  * Export the Digital Painting asset from Unity (we should make releases so this is not necessary)
  * Import the Digital Painting asset into your project
  * Open the demo scene
  * Add the `Digital Painting/Prefabs/DigitalPaintingManager`
  * Create an empty object with children that enclose the area your AI should move within (the children need non-kinematic rigid bodies and colliders). If you call it `AI Barriers` then the agent will automatically pick it up, if you need to use a different name drag it into the `Barriers` field of the controller component on your agent (called Drone by default). See `Prefabs/Environment/AI Barriers` for an example.

That's all you need to get started. Hit play and see what happens.

## Next Steps

  * Add a [Day Night Cycle](./DayNightCycle.md)
  * Mark some [Interesting Things](./InterestingThings.md) for the camera to investigate
  





