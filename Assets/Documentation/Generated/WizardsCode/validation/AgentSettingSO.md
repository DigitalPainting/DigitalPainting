# WizardsCode.Validation.AgentSettingSO

## Agent Name (String)

The name of the game object for this UMA agent.

Default Value     : "Agent"


## Talking Animation Parameter (StringReference)

The name of a boolean value that is used to play the Talking animations.

Default Value     : Talking


## Sitting Animation Parameter (StringReference)

The name of a boolean parameter that is used to indicate if the character is sitting down.

Default Value     : Sitting


## Speed Parameter (String)

The Speed parameter in the animation controller. This will be changed to reflect the current move speed of the character.

Default Value     : "Speed"


## Turn Parameter (String)

The Direction parameter in the animation controller. This will be changed to reflect the current move direction of the character.

Default Value     : "Direction"


## Look At Name (String)

Name of look at target in the prefab.


## Camera Aim Mode (CameraAimMode)

The look at camera type

Default Value     : Composer


## Camera Follow Offset (Vector3)

Camera offset from agent.

Default Value     : (0.0, 0.0, 0.0)


## Add To Scene (Boolean)

If the suggested value is a prefab should a copy of the object be added to the scene.

Default Value     : True


## Spawn Position (Vector3)

The spawn location for the prefab.

Default Value     : (0.0, 0.0, 0.0)


## Setting Name (String)

A human readable name for this setting.


## Nullable (Boolean)

Is a null value allowable? Set to true if setting can left unconfigured.

Default Value     : False


## M_Suggested Value (Object)

The suggested value for the setting. Other values may work, but if in doubt use this setting.

