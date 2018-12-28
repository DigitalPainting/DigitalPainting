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

