using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.agent.movement;
using wizardscode.digitalpainting.agent;
using wizardscode.utility;

namespace wizardscode.ai
{
    /// <summary>
    /// The FlyingMovementBrain decides where an agent is going to move to next.
    /// It does not manage the actual movement, only setting the next waypoint to
    /// move to.
    /// </summary>
    public class FlyingMovementBrain : BaseMovementBrain
    {
        private Transform wanderTarget;
        internal float timeToNextWanderPathChange = 0;

        private RobotMovementController pathfinding;

        /// <summary>
        /// Indicates whether the agent should spend time seeking Points of Interest.
        /// </summary>
        /// <returns>True if the agent should seek POIs.</returns>
        public bool SeeksPOI()
        {
            return ((AIMovementControllerSO)MovementController).seekPointsOfInterest;
        }

        /// <summary>
        /// The current waypoint, which is a point between the current location and the final Target.
        /// The last waypoint is the Target itself.
        /// </summary>
        public override Vector3 WayPointPosition
        {
            get { return pathfinding.CurrentTargetPosition; }
        }

        /// <summary>
        /// Indicates whether the agent had reached the target. To have reached the target it
        /// must be within minReachDistance.
        /// </summary>
        /// <returns>True if agent is within the minReachDistance of the current target. Also true if there is no target.</returns>
        public bool HasReachedTarget() {
            if (Target == null)
            {
                return true;
            }
            else
            {
                return Vector3.Distance(transform.position, Target.position) <= MovementController.minReachDistance;
            }
        }

        internal void Start()
        {
            pathfinding = GetComponent<RobotMovementController>();
            if (pathfinding == null)
            {
                Debug.LogWarning("No RobotMovementController found on " + gameObject.name + ". One has been added automatically, but consider adding one manually so that it may be optimally configured.");
                pathfinding = gameObject.AddComponent<RobotMovementController>();
            }
        }

        /// <summary>
        /// Typically the UpdateMove method is called from the Update method of the agent controller.
        /// It is responsible for making a decision about the agents next move and acting upon
        /// that decision.
        /// </summary>
        internal override void UpdateMove()
        {
            timeToNextWanderPathChange -= Time.deltaTime;

            if (Target == null || Vector3.Distance(transform.position, wanderTarget.position) <= MovementController.minReachDistance)
            {
                UpdateWanderTarget();
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
        private void UpdateWanderTarget()
        {
            if (wanderTarget == null)
            {
                wanderTarget = ObjectPool.Instance.GetFromPool().transform;
            }

            if (timeToNextWanderPathChange < 0)
            {
                Vector3 position = GetValidWanderPosition();
                if (position == Vector3.zero)
                {
                    // Was unable to find a valid position in a few tries so skipping for now, will retry on next frame
                    Debug.LogWarning("Unable to find a valid wander target");
                    return;
                }

                wanderTarget.position = position;

                Target = wanderTarget;
                timeToNextWanderPathChange = Random.Range(((AIMovementControllerSO)MovementController).minTimeBetweenRandomPathChanges, ((AIMovementControllerSO)MovementController).maxTimeBetweenRandomPathChanges);
            }
        }
        private Vector3 GetValidWanderPosition()
        {
            Vector3 position;
            float minDistance = ((AIMovementControllerSO)MovementController).minDistanceOfRandomPathChange / 2;
            float maxDistance = ((AIMovementControllerSO)MovementController).maxDistanceOfRandomPathChange / 2;

            float x = Random.Range(minDistance, maxDistance);
            float z = Random.Range(minDistance, maxDistance);

            position = new Vector3(transform.position.x + x, 0, transform.position.z + z);
            if (Random.value > 0.5)
            {
                position += transform.position;
            } else
            {
                position -= transform.position;
            }

            // calculate the new height 
            float terrainHeight = Terrain.activeTerrain.SampleHeight(position);
            float y = Random.Range(MovementController.minimumFlyHeight, MovementController.maximumFlyHeight);
            position.y = y + terrainHeight;

            if (!pathfinding.Octree.IsTraversableCell(position))
            {
                return GetValidWanderPosition();
            }
            else
            {
                return position;
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            if (pathfinding != null)
            {
                Octree.OctreeElement node = pathfinding.Octree.GetNode(transform.position);
                if (node != null)
                {
                    node.DrawGizmos();
                }
            }

            if (Target != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(Target.transform.position, 1);
            }

            if (WayPointPosition != null)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawSphere(WayPointPosition, 0.5f);
            }
        }
    }
}
