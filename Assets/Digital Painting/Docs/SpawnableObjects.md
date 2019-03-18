# Spawnable Objects

Included in the Digital Painting asset is a `SimpleSpawner` which can be used to spawn objects at startup.

The objects to be spawned are defined as Scriptable Objects using `Assets -> Create -> Wizards Code -> Environment -> Spawnable Object`.

# Creating a Specialist Spawner

Sometimes when spawning objects you will want to create more complex spawning behaviour. This is easy to do. You can extend the `SimpleSpawner` class and override the `Awake` method to create your own spawning algorithm. You can also override the `CustomizeObject` method in order to do per object customizations. In addition you can extend the `SpawnableObject` class to provide additional configuration parameters for your spawner,

