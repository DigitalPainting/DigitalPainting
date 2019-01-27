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
        private Transform wanderTarget;
        private Rigidbody rigidbody;

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
                if (thingOfInterest != null)
                {
                    if (Vector3.Distance(transform.position, thingOfInterest.transform.position) > thingOfInterest.distanceToTriggerViewingCamera)
                    {
                        pathfinding.Target = thingOfInterest.gameObject;
                    } else
                    {
                        ViewPOI();
                    }
                } else
                {
                    UpdatePointOfInterest();
                    if (thingOfInterest == null)
                    {
                        UpdateWanderTarget();
                    }
                    if (Vector3.Distance(transform.position, wanderTarget.position) <= pathfinding.minReachDistance)
                    {
                        timeToNextWanderPathChange = 0;
                        UpdateWanderTarget();
                    }
                    if (!pathfinding.HasTarget)
                    {
                        UpdateWanderTarget(true);
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
        private void UpdateWanderTarget(bool turnAround = false)
        {
            timeToNextWanderPathChange -= Time.deltaTime;
            if (timeToNextWanderPathChange < 0)
            {
                int minDistance = 5;
                int maxDistance = 15;
                Quaternion randAng;
                if (!turnAround)
                {
                    randAng = Quaternion.Euler(0, Random.Range(minAngleOfRandomPathChange, maxAngleOfRandomPathChange), 0);
                    
                } else
                {

                    randAng = Quaternion.Euler(0, Random.Range(180 - minAngleOfRandomPathChange, 180 + maxAngleOfRandomPathChange), 0);
                    minDistance = maxDistance;
                }
                randAng = transform.rotation * randAng;
                Vector3 position = transform.position + randAng * Vector3.forward * Random.Range(minDistance, maxDistance);

                // calculate the new height 
                float newY = Terrain.activeTerrain.SampleHeight(position) + heightOffset;
                float oldY = position.y;
                float diffY = newY - oldY;
                position.y += diffY * Time.deltaTime;

                wanderTarget.position = position;
                pathfinding.Target = wanderTarget.gameObject;
                timeToNextWanderPathChange = Random.Range(minTimeBetweenRandomPathChanges, maxTimeBetweenRandomPathChanges);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(new Ray(transform.position, rigidbody.velocity));
        }
    }
}
