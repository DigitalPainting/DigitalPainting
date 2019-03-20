using UnityEngine;

namespace wizardscode.agent.movement
{
    /// <summary>
    /// Movement Controllers implement this class and can then be assigned to a Movement Brain.
    /// </summary>
    public abstract class MovementControllerSO : ScriptableObject
    {
        [Header("Movement")]
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

        internal Transform target; // the current target that we are moving towards

        abstract internal void Move(Transform transform);
    }
}
