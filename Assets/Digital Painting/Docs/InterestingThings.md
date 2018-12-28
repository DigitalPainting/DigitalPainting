# Interesting Things

Interesting Things are items of interest in the scene. In a Digital Painting they may be, for example, 
a scenic view that automated cameras go an view.

## Creating an Interesting Thing

Simply add the `Thing` component to any game object. The default settings for the `Thing` 
are usually reasonable.

### Viewing Camera

Things will have a virtual camera attached automatically when they are created. However, 
it will be disabled and this will have minimal impact (other than memory) on your scenes
performance. This camera is used as a viewing camera when an agent investigates the Thing.

Be aware that the automated placement of the viewing camera is not currently very 
intelligent. It may well be obscured, if this is the case then either send us a patch to 
improve the positioning of the camera (preferable) or add a `CinemachineVirtualCamera` 
at an ideal position to view the object and attach this to `Viewing Camera` property of
the `Thing` component.

### Size of Things

For some activities it is important to have a collider attached to the `Thing`. For example,
when deciding where to place the viewing camera, the bounds of the collider are used. Ideally,
therefore, your `Thing` will have an appropriate collider attached. However, if there isn't
one the engine will attempt to create one. This may be wildly inaccurate, especially if your
thing is actually an empty game object marking a position (e.g. a view) rather than a physical 
thing (e.g. a bird).

