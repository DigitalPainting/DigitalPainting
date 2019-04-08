using UnityEngine;

namespace wizardscode.agent.movement
{
    /// <summary>
    /// Movement Controllers implement this class and can then be assigned to a Movement Brain.
    /// </summary>
    public class MovementControllerSO : ScriptableObject
    {
        [Header("Viewing")]
        [Header("Positioning")]
        [Tooltip("The height above the terrain this agent should be.")]
        public float heightOffset = 0;

        [Header("Movement")]
        [Tooltip("Should this agent use Root Motion from the animator?")]
        public bool useRootMotion = false;
        [Tooltip("Acceleration under normal circumstances.")]
        public float Acceleration = 1;
        [Tooltip("Walking speed under normal circumstances")]
        public float normalMovementSpeed = 1;
        [Tooltip("The factor by which to multiply the walking speed when moving fast.")]
        public float fastMovementFactor = 4;
        [Tooltip("The factor by which to multiply the walking speed when moving slowly.")]
        public float slowMovementFactor = 0.2f;
        [Tooltip("The maximum speed that the agent can move at.")]
        public float maxSpeed = 4;
        [Tooltip("Speed at which the agent will rotate.")]
        public float maxRotationSpeed = 90;
        [Tooltip("Minimum distance the agent needs to be from a position to be considered at that position.")]
        public float minReachDistance = 1;

        [Header("Flying")]
        [Tooltip("Can this agent fly?")]
        public bool canFly = true;
        [Tooltip("When flying should the agent climb / fall to track the contours of the land?")]
        public bool trackContoursInFlight = false;
        [Tooltip("Minimum flying height for this agent, if the agent goes below this height it will land.")]
        public float minimumFlyHeight = 2;
        [Tooltip("Maximum flying height for this agent, the agent will not go higher than this.")]
        public float maximumFlyHeight = 50;
        [Tooltip("Maximum flying angle for this agent.")]
        public float maximumFlyingAngle = 30;
        [Tooltip("Speed at which the agent will climb/drop in flight. Set to 0 if you don't want them to fly.")]
        public float climbSpeed = 1;

        [Header("Flying Animations")]
        [Tooltip("The name of the bool to trigger a landing from idle animation.")]
        public string idleLand = "idleLand";
        [Tooltip("The name of the bool to trigger a landing while moving animation.")]
        public string movingLand = "runLand";
        [Tooltip("The name of the bool to trigger the take off from idle animation.")]
        public string idleTakeOff = "idleTakeoff";
        [Tooltip("The name of the bool to trigger the take off while moving animation.")]
        public string movingTakeOff = "runTakeoff";
        [Tooltip("Animation to use when starting to dive towards the ground at a steep angle.")]
        public string startDive = "flyDive";
        [Tooltip("Animation to use when ending a dive.")]
        public string endDive = "flyDiveEnd";
        [Tooltip("Animation to use when starting to glide.")]
        public string glide = "flyGlide";
        [Tooltip("Animation to use when ending a glide.")]
        public string endGlide = "flyGlideEnd";
    }
}
