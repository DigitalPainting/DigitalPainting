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
        public float normalMovementMultiplier = 0.5f;
        [Tooltip("The factor by which to multiply the walking speed when moving fast.")]
        public float fastMovementMultiplier = 1;
        [Tooltip("The factor by which to multiply the walking speed when moving slowly.")]
        public float slowMovementMultiplier = 0.2f;
        [Tooltip("Speed at which the agent will climb/drop in flight. Set to 0 if you don't want them to fly.")]
        public float climbSpeed = 1;
        [Tooltip("If you model does not have the origin at the base then the height offset will enable you to position it higher or lower.")]
        public float heightOffset = 0;
        [Tooltip("Speed at which the agent will rotate.")]
        public float maxRotationSpeed = 360;

        [Header("Air Movement")]
        [Tooltip("Allow character to fly.")]
        public bool CanFly = false;
        [Tooltip("If the target is greater than this distance away then always fly, regardless of the type of waypoint.")]
        public float ToFly = 8;
        [Tooltip("Minimum Height at which this agent will fly. Below this and the agent is considered to be landing.")]
        public float minimumFlyHeight = 1;
        [Tooltip("Maximum Height at which this agent will fly.")]
        public float maximumFlyHeight = 50;
    }
}
