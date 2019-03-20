using UnityEngine;
using wizardscode.digitalpainting;

namespace wizardscode.agent.movement
{
    /// <summary>
    /// The movement brain controls movement of an agent. Through the configuration of various settings
    /// you can create a variety of movement types from fully manual to fully automated.
    /// </summary>
    [CreateAssetMenu(fileName = "Flying AI Movement Controller", menuName = "Wizards Code/Agent/Flying AI Movement Controller")]
    public class FlyingAIMovementControllerSO : MovementControllerSO
    {
        [Header("Wander configuration")]
        [Tooltip("Minimum time between random variations in the path.")]
        [Range(0, 120)]
        public float minTimeBetweenRandomPathChanges = 5;
        [Tooltip("Maximum time between random variations in the path.")]
        [Range(0, 120)]
        public float maxTimeBetweenRandomPathChanges = 15;
        [Tooltip("Minimum angle to change path when randomly varying")]
        [Range(-180, 180)]
        public float minAngleOfRandomPathChange = -25;
        [Tooltip("Maximum angle to change path when randomly varying")]
        [Range(-180, 180)]
        public float maxAngleOfRandomPathChange = 25;
        [Tooltip("Minimum distance to set a new wander target.")]
        [Range(1, 100)]
        public float minDistanceOfRandomPathChange = 10;
        [Tooltip("Maximum distance to set a new wander target.")]
        [Range(1, 100)]
        public float maxDistanceOfRandomPathChange = 25;

        internal DigitalPaintingManager manager;
        // FIXME: RobotPathfindingController should be a part of this object. Need to handle FixedUpdate
        internal RobotMovementController pathfinding;

        internal Transform wanderTarget;
        internal float timeToNextWanderPathChange = 3;

        /// <summary>
        /// Typically the Move method is called from the Update method of the agent controller.
        /// It is responsible for making a decision about the agents next move and acting upon
        /// that decision.
        /// <paramref name="transform">The transform of the agent to be moved.</paramref>
        /// </summary>
        override internal void Move(Transform transform)
        {
            if (target != null)
            {
                if (!GameObject.ReferenceEquals(pathfinding.Target, target))
                {
                    pathfinding.Target = target;
                    timeToNextWanderPathChange = 0;
                    return;
                }
            }
            else
            {
                UpdateWanderTarget(transform);

                if (Vector3.Distance(transform.position, wanderTarget.position) <= pathfinding.minReachDistance)
                {
                    UpdateWanderTarget(transform);
                }
            }
        }

        /// <summary>
        /// Update the wander target, if it is time to do so.
        /// A new position for the target is chosen within a cone defined by the
        /// minAngleOfRandomPathChange and maxAngleOfRandomPathChange. Optionally,
        /// the cone can extend behind the current agent, which has the effect of 
        /// turning the agent around.
        /// </summary>
        /// <param name="turnAround">Position the target behind the agent. By default this is false.</param>
        private void UpdateWanderTarget(Transform transform)
        {
            timeToNextWanderPathChange -= Time.deltaTime;
            if (timeToNextWanderPathChange < 0)
            {
                Vector3 position = GetValidWanderPosition(transform, 0);

                if (position == Vector3.zero)
                {
                    // Was unable to find a valid position in a few tries so skipping for now, will retry on next frame
                    Debug.LogWarning("Unable to find a valid wander target");
                    return;
                }

                wanderTarget.position = position;

                pathfinding.Target = wanderTarget;
                timeToNextWanderPathChange = Random.Range(minTimeBetweenRandomPathChanges, maxTimeBetweenRandomPathChanges);
            }
        }

        private Vector3 GetValidWanderPosition(Transform transform, int attemptCount)
        {
            int maxAttempts = 6;
            bool turnAround = false;

            attemptCount++;
            if (attemptCount > maxAttempts / 2)
            {
                turnAround = true;
            }
            else if (attemptCount > maxAttempts)
            {
                return Vector3.zero;
            }

            Vector3 position;
            float minDistance = minDistanceOfRandomPathChange;
            float maxDistance = maxDistanceOfRandomPathChange;

            Quaternion randAng;
            if (!turnAround)
            {
                randAng = Quaternion.Euler(0, Random.Range(minAngleOfRandomPathChange, maxAngleOfRandomPathChange), 0);
            }
            else
            {
                randAng = Quaternion.Euler(0, Random.Range(180 - minAngleOfRandomPathChange, 180 + maxAngleOfRandomPathChange), 0);
                minDistance = maxDistance;
            }
            transform.rotation = transform.rotation * randAng;
            position = transform.position + randAng * Vector3.forward * Random.Range(minDistance, maxDistance);

            // calculate the new height 
            float terrainHeight = Terrain.activeTerrain.SampleHeight(position);
            float newY = Mathf.Clamp(position.y, terrainHeight + pathfinding.minFlightHeight, terrainHeight + pathfinding.maxFlightHeight);
            position.y = newY;

            if (attemptCount <= maxAttempts && !pathfinding.Octree.IsTraversableCell(position))
            {
                // Debug.LogWarning("Attempt " + attemptCount + " invalid wander location: " + position);
                position = GetValidWanderPosition(transform, attemptCount);
            }

            return position;
        }

        public void OnEnable()
        {
            wanderTarget = new GameObject("Wander Target for " + name).transform;
        }
    }
}