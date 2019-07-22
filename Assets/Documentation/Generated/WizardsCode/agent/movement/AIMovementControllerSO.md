# WizardsCode.Agent.movement.AIMovementControllerSO

## Min Time Between Random Path Changes (Single)

Minimum time between random variations in the path.

Default Value     : 5
Range             : 0 and 120


## Max Time Between Random Path Changes (Single)

Maximum time between random variations in the path.

Default Value     : 15
Range             : 0 and 120


## Min Angle Of Random Path Change (Single)

Minimum angle to change path when randomly varying

Default Value     : -25
Range             : -180 and 180


## Max Angle Of Random Path Change (Single)

Maximum angle to change path when randomly varying

Default Value     : 25
Range             : -180 and 180


## Min Distance Of Random Path Change (Single)

Minimum distance to set a new wander target.

Default Value     : 10
Range             : 1 and 100


## Max Distance Of Random Path Change (Single)

Maximum distance to set a new wander target.

Default Value     : 25
Range             : 1 and 100


## Seek Points Of Interest (Boolean)

Should this agent seek points of interest in the scene?

Default Value     : True


## Minimum Reach Distance (Single)

The minimum distance from a waypoint before it is considered to have been reached.

Default Value     : 3


## Normal Movement Multiplier (Single)

Walking speed under normal circumstances

Default Value     : 0.5


## Fast Movement Multiplier (Single)

The factor by which to multiply the walking speed when moving fast.

Default Value     : 1


## Slow Movement Multiplier (Single)

The factor by which to multiply the walking speed when moving slowly.

Default Value     : 0.2


## Climb Speed (Single)

Speed at which the agent will climb/drop in flight. Set to 0 if you don't want them to fly.

Default Value     : 1


## Height Offset (Single)

If you model does not have the origin at the base then the height offset will enable you to position it higher or lower.

Default Value     : 0


## Max Rotation Speed (Single)

Speed at which the agent will rotate.

Default Value     : 360


## Can Fly (Boolean)

Allow character to fly.

Default Value     : False


## To Fly (Single)

If the target is greater than this distance away then always fly, regardless of the type of waypoint.

Default Value     : 8


## Minimum Fly Height (Single)

Minimum Height at which this agent will fly. Below this and the agent is considered to be landing.

Default Value     : 1


## Maximum Fly Height (Single)

Maximum Height at which this agent will fly.

Default Value     : 50

