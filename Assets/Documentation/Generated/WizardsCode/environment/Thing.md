# wizardscode.environment.Thing

## Is Grounded (Boolean)

Should the object be grounded? If set to true the object will be placed on the ground when it is created.

Default Value     : True


## Y Offset (Single)

Y offset to be used when positioning the thing automatically.

Default Value     : 0


## _Agent Viewing Transform (Transform)

Position and rotation the agent should adopt when viewing this thing. If null a location will be automatically created.


## Time To Look At Object (Single)

Time camera should spend paused looking at an object of interest when within range.

Default Value     : 15


## _Guid (Guid)

No tooltip provided.

Default Value     : 00000000-0000-0000-0000-000000000000


## _Virtual Camera (CinemachineVirtualCameraBase)

Virtual Camera to use in this trigger zone. If the this collider is triggered currently in focus agent then the camera will switch to this one, with the LookAt set to the agent. This can be either a prefab that will be instantiated and placed at a default location or it will be a reference to a camera pre-positioned in the scene.

