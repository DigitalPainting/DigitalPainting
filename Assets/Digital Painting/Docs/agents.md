# Agents

Agents are characters in the game that interact with the world in some way. They will, sometimes, have the focus of the camera so that we can watch what they are doing.

## Create An Agent

In the following sections we will walk through creating a new agent. This agent will fly through our scene. It will fly randomly, never with a purpose.

### The Model

Create your agents model and import it into the scene. We will simply create a Sphere and call it "Wandering Sphere".

Ensure the sphere is within the bounds of your terrain and is above ground.

### Agent Controllers

The agent controllers are the brains of the agent. They decide on what actions the agent will take. The most simple is the BaseAgentController which provides the bare minimum of functionality.

Since we want our Wandering Sphere to navigate itself around our world we will use the AIAgentController component. This includes logic that tells it how to decide where to move to and how to get there. Add this to the game object.

### Movement Controller

While the Agent Controller has logic for controlling the agents movement it does not have the configuration for that movement. You will need to tell the agent how to move around the scene, this is done with a Movement Controller Scriptable Object. We are creating a new kind of agent and so we will create a new Scriptable Object.

Right click on the `ScriptableObjects/Agents` folder and select `Wizards Code -> Agent -> Flying AI Movement Controller`. This controller provides configuration options used by the movement AI. Here you can configure how the agent moves.

We'll leave most items at their default, but we will want to change the `seekPointsOfInterest` property to false. This controls whether the agent will actively seek out points of interest in the world. Since we want our agent to simply wander around we'll turn this off.

Drop this scriptable object into the MovementController slot of you agent Game Object.

### Spawnging Agents

That is it (well not quite). You can now start your application and your sphere will start moving around. We don't want to have to manually manage all agents in the scene. In order to do this we will use the DigitalPaintingManager. Increase the size of the AgentObjectDefs property by 1.

We need another scriptable object to put in here to define our new agent. To create this right click on the `ScriptableObjects/Agents` folder and select `Wizards Code -> Agent -> Agent Definition`. Name the new Scriptable Object "Wandering Sphere". To configure this definition we need a prefab for the model. Create one our of the object we created in the scene and delete the original from the scene. Add the prefab to the Agent Definition.

Now that we have our Agent Definition Scriptable Object we can drag this into the new Agent Object Defs slot we created before.

Now when you play your scene your wandering sphere will be spawned in.

