# Cameras

By default your scene will have a follow camera that will follow the "in focus" agent. You can overwrite this by setting the `defaultCameraRig` in the Director properties. 

You can change the follow target by setting the value of the SO Architecture variable `AgentWithFocus`. Under normal circumstances the Director will ensure active cameras are tracking this agent. However, the Director will sometimes choose to have cameras view different items in the world.

This default camera works pretty well in open spaces, but it can have trouble in areas with lots of obstructions, such as in a building. It is therefore possible to create specific cameras that will be triggered in certain circumstances.

For example, in the main demo scene there is a "maze" that uses a `Cinemachine ClearShot` camera to ensure the agent is always in view. You can use any CineMachine camera, not just a ClearShot. Also, in the `Demo` scene there are come trigger cameras that will be fired when something enters the area they are covering. How trigger cameras work will be describe below.

# Test Scene

In the `Scenes/Assets/DevTest` folder you will find a `Cameras` scene. This has a simple scene that is used for testing camera behaviour. In this scene you can find a number of examples of how to use different kinds of camera in your Digital Painting. 

# Trigger Cameras

A trigger camera is a camera that has a trigger collider attached and a `CameraTrigger` component. When something triggers the collider the `CameraTrigger` script adjusts the priority of the specified camera. Cinemachine will then use the priority to decide which camera to use.

By default the ClearShot camera set up to follow the agent (see above) has a priority of 100. Therefore, having the `CameraTrigger` increase the priority by less than 100 will not change the camera in use. However, it is possible there are other influences on a cameras priority. These influences are cumulative. 

## Creating Trigger Cameras

Create a GameObject that will act as the parent for Camera Trigger. Add a `Collider` with `Is Trigger` set to true that covers the area within which you want the new camera to be active. Note, that you cannot attach the collider to the Virtual Camera because the camera will move when tracking the agent and thus will move the collider.

Create a Cinemachine Camera, I like to make mine a child of the GameObject so that they will move with the object, but you put them wherever you like.

Add the `CameraTrigger` component to the game object that you attached the collider to and drag the Virtual Camera into the `Virtual Camera` field in the `Trigger Reaction` section.

Optionally you can add an SO Architecture GameEvent to the `On Enter Event` and/or the `On Exit Event` properties. These events will be fired whenever the camera is triggered. If the only effect you desire is to increase the priority of the camera then there is no need to have events attached here. However, they can be useful if you want to do some additional work.


