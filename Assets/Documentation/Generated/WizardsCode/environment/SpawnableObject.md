# wizardscode.environment.SpawnableObject

## Prefab (GameObject)

Prefab to spawn.


## Min Size (Single)

Minimum size. All objects spawned will be a size of (1, 1, 1) this value.

Default Value     : 1


## Create Parent (Boolean)

Create a parent object to contain the spawned objects?

Default Value     : True


## Is Interesting Thing (Boolean)

Is the parent an Interesting Thing in the environment. Interesting things will be examined by agents within the world. Not used if there is no parent object.

Default Value     : True


## Center (Vector3)

Position of the center point of the spawn area.

Default Value     : (0.0, 0.0, 0.0)


## Radius (Single)

Radius within which to spawn the objects.

Default Value     : 10


## Is Grounded (Boolean)

Is the item to be spawned at ground level?

Default Value     : True


## Number (Int32)

Number to be spawned.

Default Value     : 10


## Y Offset (Single)

Additional Y offset. This can be used to do things like ensure the trunk of a tree completely penetrates the ground.

Default Value     : 0


## Random Angle (Int32)

Maximum angle of rotation. The object will be spawned at an angle of 0 to this value.

Default Value     : 0
Range             : 0 and 360

