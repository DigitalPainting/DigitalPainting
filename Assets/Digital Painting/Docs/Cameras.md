# Cameras

By default your scene will have a follow camera that will follow the "in focus" agent. You can change the follow target by setting `DigitalPaintingManager.AgentWithFocus`. This default camera works pretty well in open spaces, but it can have trouble in areas with lots of obstructions, such as in a building. It is therefore possible to create specific cameras that will be triggered in certain circumstances.

For example, in the `Demo Scene` there is a "maze" that uses a `Cinemachine ClearShot` camera to ensure the agent is always in view. You can use any CineMachine camera, not just a ClearShot.

# Creating Specific Cameras

First add the `Cinemachine ClearShot` camera to your scene and configure it however you like.

Next disable the `Cinemachine ClearShot` component. we only want it to be active under specific conditions, in this case whenever the agent is inside, or about to enter, the maze.

Add a `Collider` with `Is Trigger` set to true that covers the area within which you want the new camera to be active.

Add the `CameraTrigger` components and drag the Virtual Camera into the `Virtual Camera` field in the `Agent Interaction` section.


