# Agents

Agents are characters in the game that interact with the world in some way. They will, sometimes, have the focus of the camera so that we can watch what they are doing.

## Create A Simple Agent

In the following sections we will walk through creating a new agent. This agent will fly through our scene. It will fly randomly, never with a purpose.

### The Model

Create your agents model and import it into the scene. We will simply create a Sphere and call it "Wandering Sphere".

Ensure the sphere is within the bounds of your terrain and is above ground.

### Agent Controllers

The agent controllers are the brains of the agent. They decide on what actions the agent will take. The most simple is the BaseAgentController which provides the bare minimum of functionality, including simple WASD movement controls.

Since we want our Wandering Sphere to navigate itself around our world we will use the AIAgentController component. This includes logic that tells it how to decide where to move to and how to get there. Add this to the game object.

### Movement Controller Configuration

While the Agent Controller has logic for controlling where the agent wants to move to and how it moves, it does not have the configuration for that movement (e.g. how fast it moves). This is stored in a Scriptable Object rather than the Movement Controller to allow reuse of both the components in different ways. The configuration for the movement is stored in a Movement Controller Scriptable Object. Since we are creating a new kind of agent and  we will create a new Scriptable Object. This allows us to reuse the AIAgentController while having it move differently from other agents that use this controller.

Right click on the `ScriptableObjects/Agents` folder (create it if necessary) and select `Wizards Code -> Agent -> Flying AI Movement Controller`. This controller provides configuration options used by the movement AI. Here you can configure how the agent moves.

We'll leave most items at their default, but we will want to change the `seekPointsOfInterest` property to false. This controls whether the agent will actively seek out points of interest in the world. Since we want our agent to simply wander around we'll turn this off.

Drop this scriptable object into the MovementController slot of you agent Game Object.

### Spawnging Agents

That is it (well not quite). You can now start your application and your sphere will start moving around. If you hare happy to have your agents defined in the scene itself you can move on now. However, it is sometimes a good idea to create a prefab and have the Digital Painting code spawn your agents at startup.

Create a prefab for your agent.

Agents spawned by the Digital Painting Manager are defined by another scriptable object. To create this right click on the `ScriptableObjects/Agents` folder and select `Wizards Code -> Agent -> Agent Definition`. Name the new Scriptable Object "Wandering Sphere". Add the prefab created in the previous step to the Agent Definition.

In the `DigitalPaintingMaanger` in your scene increase the size of the AgentObjectDefs property by 1 and put your agent definition scriptable object into the created slot.

Now when you play your scene your wandering sphere will be spawned in.

## Create an Advanced Agent

In this section we'll look at how to create a much more advanced agent using an Asset from the store. Each asset you want to use will be different, therefore it is impossible to give a truly comprehensive reference on how to include purchased assets in your paintings. However, this walkkthrough is designed to give you enough insight into how things work to get you started.

We'll use the excellent [Dragons Pack](https://assetstore.unity.com/packages/3d/characters/creatures/dragons-pack-pbr-45330) from [Infinity PBR](http://www.infinitypbr.com/). This pack comes with an extensive range of animations, actions and sounds. This is a paid asset (and worth every penny).

Start by [creating a demo](./CreatingAScene.md) scene with Digital Painting enabled. Now import the Dragons Pack asset and add the Dragon to your scene.

## Create a Custom Agent Controller

The dragon model has some great animations with root motion. We want to use those to move our agent. To do this we will need a Movement Controller for our dragon. To keep things simple we'll start by controlling the Dragon with the keyboard. Later we'll hook create an AI Agent Controller.

Create a new script called `DragonAgentController` and have it extend `BaseFlyingAgentController` provided in the Digital Painting Asset (it's in the `wizardscode.agent` namespace).  Add the `DragonAgentController` to the Dragon. This is going to be empty for this walkthrough. We have created it so that we have an easy way of changnig behaviour without impacting the base flying behaviour as we continue to work on the Dragon.

## Movement Controller Configuration

As with the previous example we need to define the parameters that control the dragons movements. We do this by providing a Movement Controller Scriptable Object. Create a ScriptableObjects folder in your Scripts directory and right click it, then select `Wizards Code -> Agent -> Manual Movement Controller`. Call it `DragonMovementController` This controller provides configuration options used by the most basic of movement types. Most of the default settings will do for now, but you should set `Use Root Motion` to true since the Dragon's animations have root motion. We'll also ensure that all the animation triggers identified in this controller are correctly set. They should be "idleLand", "runLand", "idleTakeoff" and "runTakeoff".

Drag your movement controller into the approrpiate field in the `DragonAgentController` as this will be the default controller.

## Custom Camera

We'd like to have a custom camera for this kind of agent. Let's start from the default ClearShot camera provided in the Digital Painting. Drag this clearshot camera into your scene, unpack the prefab and rename the object to `Dragon Clearshot`. Create a prefab from this camera and delete it from your scene. Adjust the camera settings as desired.

We need to tell the Director to use this camera as the default whenver the Dragon is the agent with focus. To do this we simply drag the prefab into the Virtual Camera Prefab proeprty of the Dragon Agent Controller.

## Spawning the Dragon

Lets have the Digital Painting Manager spawn our Dragon so that it can setup the camera for us. Create a prefab from your dragon and remove it from your scene. Now create the definiton object by right clicking on the `ScriptableObjects/Agents` folder and selecting `Wizards Code -> Agent -> Agent Definition`. Name the new Scriptable Object "Dragon Definition". 

Set the prefab and movement controller to the objects you just created. Note that you don't really need to provide the Movement Controller here since we are just using the default. However, for completeness it is a good idea to provide it. If you provide a different controller here it will override the one identified in the prefab.

Create a slot in the agents definition list in the Digital Painting Manager and drag the definition into it.

## Animations

Basic animations are handled by the BaseFlyingAgentController, so there  is nothing for you to do. Though you will likely need to set the `Minimum Fly Height` in the Movement Controller SO. This is used to ensure that the agent is at the right height when the landing animation is started. It is also used to ensure the model rises to a height that will prevent it clipping the ground as it flies.

## AI Controller

Now that we have the basics setup for manual control lets create a stimple AI controller. Create a new `AIDragonAgentController` and have it inherit from `AIAgentController`.

## Prefab

Duplicate the dragon prefab and add the AIDragonController to the new one. Setup the controller as above.

## Spawn the dragon

We need to use this new controller with our dragon. We could create a new prefab but it's possible