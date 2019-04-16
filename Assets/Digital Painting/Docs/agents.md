# Agents

Agents are characters in the game that interact with the world in some way. They will, sometimes, have the focus of the camera so that we can watch what they are doing.

## Create A Simple Agent

In the following sections we will walk through creating a new agent. This agent will fly through our scene. It will fly randomly, never with a purpose. This is a very simplistic agent with no animations. In a subsequent sections we'll walk through creating an agent from an asset with a character controller and animations.

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

### Spawngng Agents

That is it (well not quite). You can now start your application and your sphere will start moving around. We don't want to have to manually manage all agents in the scene. In order to do this we will use the DigitalPaintingManager. Increase the size of the AgentObjectDefs property by 1.

We need another scriptable object to put in here to define our new agent. To create this right click on the `ScriptableObjects/Agents` folder and select `Wizards Code -> Agent -> Agent Definition`. Name the new Scriptable Object "Wandering Sphere". To configure this definition we need a prefab for the model. Create one our of the object we created in the scene and delete the original from the scene. Add the prefab to the Agent Definition.

Now that we have our Agent Definition Scriptable Object we can drag this into the new Agent Object Defs slot we created before.

Now when you play your scene your wandering sphere will be spawned in.

## Create an Agent with a Character Controller

For the most part we want our agents to be more complex. They will have a character controller and animations. Precisely how you wire up these agents will vary depending on the character controller in use. In this example we will use the [Elemental Dragon](https://assetstore.unity.com/packages/3d/characters/creatures/elemental-dragon-137816) from Malbers Animations.

### Statically Import Into Your Scene

Duplicate the dev scene in the Digital Painting asset and move the copy into the Scenes folder in the root of your project. Ensure the NavMesh is correctly baked. This will create a dev environment for you to work in. We'll remove the existing agents from the scene to keep things simple. Go to DigitalPaintingManager and set the size of the Agent Object Defs to 0.

Next up, import the asset into your project.

To get started we wil simply drop a default prefab (without AI) from the asset into our scene. The Elemental Dragon comes with a controller. If you hit play now you can control the dragon using the controller but it is not recognized as an Agent in the painting and so the cameras do not react to it. To fix this we need to add the `BaseAgentController` script to the dragon. Now when we play we can move around using the Controller provided by the Elemental Dragon asset. The dragon will also trigger cameras in the scene. 

We've made a few changes to the Dragon Prefab and should probably create a new prefab before we accidentally save those changes to the original. 

### Integrating the AI Controller

The beuaty of a Digital Painting is that it lives on its own. Therefore we need to enable the AI script that comes with this asset. The AI is pretty basic, but it's a start. So lets figure out how to use it in our scene.

Lets start by duplicating the prefab we created. This new prefab should be renamed to indicate it is an AI. Remove the `malbers input Script` from this version of the prefab and add the `Animal Ai Control` and, since the AI requires it, add a `NavMeshAgent`. Replace the dragon in your scene with one made from this prefab.

By default the AI uses a set of Waypoints that are fixed in the scene. This is not ideal for our final goal, but to get us started we'll use this approach. Later we'll dynamically generate Waypoints in the painting itself. For now, add a number of waypoints (Game Objects with the MWayPoint script attached). Five is a reasonable number with some in the air and some on the ground. Make each one a "Next Way Point" for all points. Make one of them the starting target for the Dragon.

Now, when you press play, your dragon will move around the scene from waypoint to waypoint.

### Visiting Points of Interest

The Digital Painting had the concept of Points of Interest. These can be thought of as Waypoints. In order to use the POI system we need a way of feeding them into the AI controller that is supplied as part of our asset. We'll do this by creating new Digital Painting Agent Controller that is aware of the Malber AI controller. It will add the appropriate Waypoint information to the POI and then feed that waypoint to the Malber controller.

To do this create a controller that extends `BaseAIAgentController` and override the `Target` set method to ensure that any selected target has an MWayPoint script and is fed to the AI controller.

FIXME: need to add a waypoint with the correct type to each object

Replace the `BaseAgentController` on the Dragon with the `MalbersAIAgentController`. For this controller to work we will need a movement controller for our dragon. You can create one using the `Create -> Assets -> Wizards Code -> Agent` menu.

Remove (or at least disable) all the fixed waypoints you created in the previous section. Set the Target in the `Animal AI Control` script to null.

