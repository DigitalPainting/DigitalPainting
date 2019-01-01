# Interesting Things

Interesting Things are items of interest in the scene. In a Digital Painting they may be, for example, 
a scenic view that automated cameras go an view.

## Creating an Interesting Thing at Design Time

Simply add the `Thing` component to any game object. The default settings for the `Thing` are usually reasonable. The most common customization that is done is to change the Virtual Camera used for viewing this Thing. See below for more details.

## Creating an Interesting Thing at Runtime

This feature is in development at the time of writing. How it works can be seen in the `InterestingThingsUI` which provides a button that adds an object at the location of the drone.

Note that things added in play mode in the editor are not persisted in the scene. If you want them to be persisted you need to copy the objects before stopping the scene and then paste them in when in the editor. This can provide a way if identifying good places for an interesting thing.

## Specifying a Virtual Camera

Things will have a virtual camera attached automatically when they are created. However, 
it will be disabled and this will have minimal impact (other than memory) on your scenes
performance. This camera is used as a viewing camera when an agent investigates the Thing.

Be aware that the automated placement of the viewing camera is not currently very 
intelligent. It may well be obscured, if this is the case then either send us a patch to 
improve the positioning of the camera (preferable) or add a `CinemachineVirtualCamera` 
at an ideal position to view the object and attach this to `Viewing Camera` property of
the `Thing` component.

### Adding a Dedicated Virtual Camera

For better positioning of the viewing camera you can create a fixed virtual camera that will
be used instead of the automated one. This allows you to frame the picture correctly.

  * Select the Interesting Thing in the scene view
  * Select the scene view and hit `f` to focus on the object
  * Position the view as you want it in game (right click and use WASDQE to move the scene)
  * Cinemachine -> Add Virtual Camera
  * Move the created virtual camera to be in a sensible place in your hierarchy (I make them a child of the interesting thing)
  * Give the virtual camera a good name
  * Drag the virtual camera into the `Virtual Camera` field of the Interesting Thing
  * Drag the InterestingThing into the `Follow` and `Look At fields` of the virtual camera
  * Set the `Follow Offset` of the virtual camera to give the same results as seen in the scene window
    * Note, I've not figured out an easy way to do this, I look at the orientation of the xyz axis in the scene view and play with the values until the Camera view looks as I want it. Don't forget to account for different screen resolutions.
  * Adjust other camera settings for the perfect image

### Size of Things

For some activities it is important to have a collider attached to the `Thing`. For example,
when deciding where to place the viewing camera, the bounds of the collider are used. Ideally,
therefore, your `Thing` will have an appropriate collider attached. However, if there isn't
one the engine will attempt to create one. This may be wildly inaccurate, especially if your
thing is actually an empty game object marking a position (e.g. a view) rather than a physical 
thing (e.g. a bird).

