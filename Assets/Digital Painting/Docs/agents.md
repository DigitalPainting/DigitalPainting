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

Create a new script called `DragonAgentController` and have it extend `BaseFlyingAgentController` provided in the Digital Painting Asset (it's in the `wizardscode.agent` namespace).  Add the `DragonAgentController` to the Dragon.

## Movement Controller Configuration

As with the previous example we need to define the parameters that control the dragons movements. We do this by providing a Movement Controller Scriptable Object. Create a ScriptableObjects folder in your Scripts directory and right click it, then select `Wizards Code -> Agent -> Manual Movement Controller`. Call it `DragonMovementController` This controller provides configuration options used by the most basic of movement types. Most of the default settings will do for now, but you should set `Use Root Motion` to true since the Dragon's animations have root motion.

Drag your mvoement controller into the approrpiate field in the `DragonAgentController`.

## Spawning the Dragon

Lets have the Digital Painting Manager spawn our Dragon so that it can setup the camera for us. Create a prefab from your dragon and remove it from your scene. Now create the definiton object by right clicking on the `ScriptableObjects/Agents` folder and selecting `Wizards Code -> Agent -> Agent Definition`. Name the new Scriptable Object "Dragon Definition". Create a slot in the agents definition list in the Digital Painting Manager and drag the definition into it.

## Custom Camera

We'd like to have a custom camera for this kind of agent. Let's start from the default Clearhot camera provided in the Digital Painting. Drag this clearshot camera into your scene, unpack the prefab and rename the object to `Dragon Clearshot`. Create a prefab from this camera and delete it from your scene. Adjust the camera settings as desired.

We need to tell the Director to use this camera as the default whenver the Dragon is the agent with focus. To do this we simply drag the prefab into the Virtual Camera Prefab proeprty of the Dragojn Agent Controller.

## Adding Animations

If you hit play now you will be able to make the dragon slide about using whatever Inputs you have setup in Unity. You can also make the Dragon fly using the 'q' and 'e' keys. However, there are no animations. Lets start with making the dragon flap its wings when in the air.

The animator for the Dragon uses a set of parameters. In the Dragons Pack demo scene these are managed by buttons and sliders on the canvas which, in turn, call various methods in the the SFB_DragonHeight. We'll use the code in this file and on the UI events, to inspire our own controller.

There is an boolean parameter in the animator called `idleTakeoff` which we will use to have the dragon take off whenever it leaves the ground and another called `idleLand` which can be used when it reaches the ground. To make this happen we need to override the `TakeOff` and `Land` methods in our `DragonAgentController`. That was easy. Now lets make the dragon fly forward. The animator supplied with the Dragon pack has an Animation Event that calls a SetGroundHeight method. Since we are not using the script containing this method we need to replace that in our `DragonAgentController`. To wire up the event we will have to make a copy of the animation controller and the IdleLand animation. Replace the IdleLand animation in our new controller with the copy of idle land and delete the SetGroundHeight event (as we will be calculating the correct ground height in the controller).

Moving forward is controlled by the `locomotion` parameter on the animator. A value of 0.5 is stationary, 0 is full speed backwards and 1 is full speed forwards. This makes it possible to pass the value of the input axis directly into the parameter. This is done by overriding the `MoveVerticalAxis` method. 
