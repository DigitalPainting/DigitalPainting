# Creating A Scene

The idea of this project is that you can use it as a base for your own Digital Paintings,
therefore you need to be able to add it to your own scene. This document describes how to
do that.

In this example we'll document using a scene purchased from the Asset Store, however you 
can use any scene that has a terrain withing it.

## Create a Base Project

Create your new project and create or import your scene. I've used the excellent [Nature 
Manufacture’s Meadow Environment – Dynamic Nature](https://assetstore.unity.com/packages/3d/vegetation/meadow-environment-dynamic-nature-132195)
asset. I'm using a copy of the `Unity Standard Demo Scene` that comes with the asset.
THe process should be the same if you want to use the gills demo scene, but that requires
Unity Pro and some other assets. 

The Meadow environment 
asset recommends you to setup some of the lighting and camera effects, but it should be
noted that some plugins for THe Digital Painting also advise specific lighting and camera 
effects. These plugins may overwrite your settings. At the time of writing there is no way to force a specific setup. This is a feature that
needs to be added, see [issue #23](https://github.com/DigitalPainting/DigitalPainting/issues/23) 
to track this. If you hit a problem in this regard we would welcome your contribution.

Once you have your scene setup as you want it's time to add the Digital Painting assets.

### Minimal Setup in your Scene

The Digital Painting asset expects your scene to have a terrain. There are also some specific
configurations you should do, but we'll come to those after we've installed the asset.

## Add the Core Digital Painting Assets and Dependencies

Having created a standard Unity scene you need to add The Digital Painting Assets. If you want to use
defaults this is pretty easy to do. You can either work from the source or from a released package.
However, we will first add the required dependencies (if you miss one of these steps you will get
compile errors when importing the Digital Painting code, don't worry, simply add them later).

  * `Window -> Package Manager -> Cinemachine -> Install`

### From Source

If you want to work from source then we recommend the use of [Git Submodules](https://git-scm.com/book/en/v2/Git-Tools-Submodules):

  * `cd Assets`
  * `git submodule add git@github.com:DigitalPainting/ScriptableObject-Architecture.git`
    * The [Scriptable Object Architecture](../../ScriptableObject-Architecture/README.md) provides a set of scripts for working with Scriptable Objects as a means to keep a loose coupling between components.
  * `git submodule add git@github.com:DigitalPainting/Flying-Pathfinding.git`
    * The [Flying Pathfinding](../../Flying-Pathfinding/README.md) asset provides a framework for to A* navigation for airborne agents.
  * `git submodule add git@github.com:DigitalPainting/DigitalPainting.git`
    * This is the core Digital Painting Asset

### From Released Packages

NOTE: At the time of writing we do not published released packages as the code below is a placeholder for when we do.

  * [Export the Digital Painting asset](ReleasingTheDigitalPaintingAsset.md) from Unity (we should make releases so this is not necessary)
  * Import the Digital Painting asset into your project
  * If there are any compile errors import the required packages (see above)

## Configuring your scene to use Digital Painting Core assets

The prefab `Assets/DigitalPainting/Prefabs/DigitalPaintingManager` includes all the required configuration components, drag it
into your scene and then work through each of the following sections. Note that the below describes the minim configuration. There 
are many items that will be automatically configured for you at runtime. This can be helpful in getting started but in most
situations you will want to manually configure these to optimize for your scene. Whenever the Digital Painting auto-configures
something for you it will log this to the Debug console.

Similarly, there are many more parameters you can adjust than those described below. With attention to detail comes more satisfactory
results.

You will also need to provide a [Flying Pathfinding](../../Flying-Pathfinding/README.md) configuration to provide the A* navigation 
for agents that fly. You can find an example prefab in `Assets/Flying-Pathfinding/Assets/Flying-Pathfinding/Prefabs/Octree` prefab. For
the most part the defaults will work fine. But it is important to ensure that the Box Collider covers the entire area you want agents 
to fly and that it is positioned correctly on your terrain.

The following section describes the required configuration for these components.

### Digital Painting Manager [Required]

The Digital Painting Manager component is a required component. It is the central control component and
is responsible for coordinating all other components. It has the following configurations:

#### Agent With Focus

This is a [variable reference](../../ScriptableObject-Architecture/README.md) that records the currently selected agent. This needs to
be set up to be the `DigitalPainting/ScriptableObjects/Agents/AgentWithFocus` Scriptable Object.

#### Agent Object Definitions [Required]

Agents are able to take actions in the world. That is they can interact with one or more elements of the world. They are not
merely scenery. The camera will be focussed on at least one agent at all times. Agents are often visible, but they need not be.
In the case of invisible agents the camera will appear to be "flying" through the world. Each painting requires at least one agent.

Agents are spawned at the start of the game according to the configuration in the `Agent Object Defs`. A number of example agents
are provided as part of the core package. A good one to start with is the `Fairy` agent, this is a glowing light that will wander
around the world looking for interesting things within it.

To add an agent:

  * Increase the size of the list of Agent Defs by 1
  * Click the selection icon and select your preferred agent definition

### Things Manager [Required]

[Things](InterestingThings.md) are items in the world that agents can interact with. They are scenery in that they don't display anything but the simplest
of behaviors, but the do add a level of depth to the world. If you are using the `Fairy` agent in your world you will find that
your fairy will seek out Things and will visit them.

The Things Manager requires no configuration.

### Director [Required]

The Director is in change of the camera work. It requires at least the following setup.

Agent with focus should be set to the same [variable reference](../../ScriptableObject-Architecture/README.md) you set elsewhere. 
In the default configuration that is the  `DigitalPainting/ScriptableObjects/Agents/AgentWithFocus` Scriptable Object.

#### Game Event Listener [Required, if there is more than one Agent in the scene]

The Game Event Listener uses an eventing model that is part of the 
[Scriptable Object Architecture](../../ScriptableObject-Architecture/README.md) asset that allowd the Director to respond to changes
in the agent focus. It requires no configuration.

### Target Object Pool [Required]

The Target Object Pool manages a pool of objects used by AI Agents for navigation. It requires no configuration.

### Day Night Cycle Manager [Optional]

By default we provide a very simple Day Night Cycle component that will manage the passage of time and crudely simulate the movement
of the Sun. It is possible to plugin other [Day Night Cycle](./DayNightCycle.md) assets. It is safe to turn this off if you don't want
a Day Night Cycle. If you choose to use it then you need to set at least the following fields:

#### Sun 

You must have a sun in your scene. By default the Day Night Cycle will pick up a directonal light called 'Sun', it is sufficient to 
rename the default light in a new scene to 'Sun'. Alternatively you can provide a prefab that will be instantiated at the start of 
the game. An example sun is provided in `Assets/DigitalPainting/Prefabs/Environment/Sun`. If you use a prefab you probably want to
remove any existing directional light in the scene.

### Weather Manager [Optional]

The weather manager simulates the weather in your world. The default setup doesn't actually display any weather effects, it simply logs
the current weather status. It isn't very useful in this form. Unless you intend to import a [Weather System](./Weather.md) asset you
should probably disable this component. Note that there is a plugin for a free rain plugin if you want to experiement with the weather
system, see the [Weather System](./Weather.md) documentation for details.

### Demo Manager [Optional]

The demo manager makes it easier to provide a demo scene. If you are not creating a demo you should remove this component.

## Play

You can now hit play and see the first version of your digital painting.

## Next Steps

  * Create your own [Custom Agent](./agents.md)
  * Add [cinematic cameras](./Cameras.md)
  * Add a more realistic [Day Night Cycle](./DayNightCycle.md)
  * Add a [Weather System](./Weather.md)
  * Mark some [Interesting Things](./InterestingThings.md) for the agents to investigate