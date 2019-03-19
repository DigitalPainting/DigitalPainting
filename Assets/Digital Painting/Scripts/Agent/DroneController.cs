using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.digitalpainting.agent
{
    /// <summary>
    /// DroneController is a FlyByWire or manual controller for a drone.
    /// 
    /// </summary>
    public class DroneController : AIAgentController
    {
        private RobotMovementController pathfinding;
        internal Transform wanderTarget;
        new private Rigidbody rigidbody;

        override internal void Awake()
        {
            base.Awake();
            pathfinding = GetComponent<RobotMovementController>();
            if (pathfinding == null)
            {
                Debug.LogWarning("No RobotMovementController found on " + gameObject.name + ". One has been added automatically, but consider adding one manually so that it may be optimally configured.");
                pathfinding = gameObject.AddComponent<RobotMovementController>();
            }
            wanderTarget = new GameObject("Wander Target for " + gameObject.name).transform;

            rigidbody = GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                Debug.LogWarning(gameObject.name + " does not have a rigidbody. One has been added automatically, but you might want to add one to the object so that it can be properly configured.");
                rigidbody = gameObject.AddComponent<Rigidbody>();
            }
        }

        internal override void Update()
        {
            if (isFlyByWire)
            {
                if (ThingOfInterest != null)
                {
                    if (!GameObject.ReferenceEquals(pathfinding.Target, ThingOfInterest.AgentViewingTransform)) {
                        pathfinding.Target = ThingOfInterest.AgentViewingTransform;
                        return;
                    }

                    if (Vector3.Distance(transform.position, ThingOfInterest.AgentViewingTransform.position) <= ThingOfInterest.distanceToTriggerViewingCamera)
                    {
                        ViewPOI();
                    }

                    /**
                    if (!pathfinding.Octree.IsBuilding)
                    {
                        if (!pathfinding.HasReachableTarget)
                        {
                            Debug.LogError("Unable to reach POI: " + ThingOfInterest.name);
                        }
                    }
    */
                } else
                {
                    UpdatePointOfInterest();
                    if (ThingOfInterest == null)
                    {
                        UpdateWanderTarget();
                    }
                    if (Vector3.Distance(transform.position, wanderTarget.position) <= pathfinding.minReachDistance)
                    {
                        timeToNextWanderPathChange = 0;
                        UpdateWanderTarget();
                    }
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
        private void UpdateWanderTarget()
        {
            timeToNextWanderPathChange -= Time.deltaTime;
            if (timeToNextWanderPathChange < 0)
            {
                Vector3 position = GetValidWanderPosition(0);

                if (position == Vector3.zero)
                {
                    // Was unable to find a valid position in a few tries so skipping for now, will retry on next frame
                    return;
                }
                
                wanderTarget.position = position;

                pathfinding.Target = wanderTarget;
                timeToNextWanderPathChange = Random.Range(minTimeBetweenRandomPathChanges, maxTimeBetweenRandomPathChanges);

                manager.SetLookTarget(wanderTarget.transform);
            }
        }

        private Vector3 GetValidWanderPosition(int attemptCount)
        {
            int maxAttempts = 6;
            bool turnAround = false;

            attemptCount ++;
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
                position = GetValidWanderPosition(attemptCount);
            }

            return position;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(new Ray(transform.position, rigidbody.velocity));
        }
    }
}
