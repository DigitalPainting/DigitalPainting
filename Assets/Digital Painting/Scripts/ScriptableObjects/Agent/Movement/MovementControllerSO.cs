using UnityEngine;

namespace wizardscode.agent.movement
{
    /// <summary>
    /// Movement Controllers implement this class and can then be assigned to a Movement Brain.
    /// </summary>
    public class MovementControllerSO : ScriptableObject
    {
        [Header("Common")]
        [Tooltip("The minimum distance from a waypoint before it is considered to have been reached.")]
        public float minimumReachDistance = 3;

        [Header("Ground Movement")]
        [Tooltip("Walking speed under normal circumstances")]
        public float normalMovementSpeed = 1;
        [Tooltip("The factor by which to multiply the walking speed when moving fast.")]
        public float fastMovementFactor = 4;
        [Tooltip("The factor by which to multiply the walking speed when moving slowly.")]
        public float slowMovementFactor = 0.2f;
        [Tooltip("Speed at which the agent will climb/drop in flight. Set to 0 if you don't want them to fly.")]
        public float climbSpeed = 1;
        [Tooltip("The height above the terrain this agent should be.")]
        public float heightOffset = 0;
        [Tooltip("Speed at which the agent will rotate.")]
        public float rotationSpeed = 90;

        [Header("Air Movement")]
        [Tooltip("Minimum Height at which this agent will fly. Below this and the agent is considered to be landing.")]
        public float minimumFlyHeight = 1;
        [Tooltip("Maximum Height at which this agent will fly.")]
        public float maximumFlyHeight = 50;
    }
}
