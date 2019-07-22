# wizardscode.production.CameraTrigger

## _Virtual Camera (CinemachineVirtualCameraBase)

Virtual Camera to use in this trigger zone. If the this collider is triggered currently in focus agent then the camera will switch to this one, with the LookAt set to the agent. This can be either a prefab that will be instantiated and placed at a default location or it will be a reference to a camera pre-positioned in the scene.


## Config (CameraTriggerConfiguration)

Configuration for this camera trigger.


## _Default Look At Target (Transform)

Default look at target, only used if the lookAtTriggerAgent in the CameraTriggerConfig is false. If this is null then the transform of the object this component is attached to will be used.

