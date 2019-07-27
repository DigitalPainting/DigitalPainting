using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.Agent.movement;
using WizardsCode.DigitalPainting.Agent;
using WizardsCode.Environment;
using WizardsCode.Utility;
using Random = UnityEngine.Random;

namespace WizardsCode.Agent
{
    public class BaseAIAgentController : BaseAgentController
    {
        [Tooltip("The range the agent will use to detect things in its environment")]
        public float detectionRange = 50;

        private List<Thing> visitedThings = new List<Thing>();
        private float timeLeftLookingAtObject = float.NegativeInfinity;

        internal Transform wanderTarget;
        internal float timeToNextWanderPathChange = 0;
        internal Transform m_target;
        private bool m_targetReached;

        /// <summary>
        /// Get or set the current target.
        /// </summary>
        virtual public Transform Target
        {
            get { return m_target; }
            set
            {
                if (m_target == value) return;
                 
                m_target = value;
                timeToNextWanderPathChange = Random.Range(MovementController.minTimeBetweenRandomPathChanges, MovementController.maxTimeBetweenRandomPathChanges);
                
                if (value)
                {
                    HasReachedTarget = false;

                    Vector3 pos;
                    Thing thing = value.GetComponent<Thing>();
                    if (thing)
                    {
                        if (thing.AgentViewingTransform)
                        {   
                            pos = thing.AgentViewingTransform.position;
                            m_target = thing.AgentViewingTransform;
                        }
                        else
                        {
                            pos = value.transform.position;
                            m_target = value.transform;
                        }

                        if (thing.isGrounded)
                        {
                            pos.y = UnityEngine.Terrain.activeTerrain.SampleHeight(pos);
                        }
                    }
                    else
                    {
                        pos = value.transform.position;
                        float height = UnityEngine.Terrain.activeTerrain.SampleHeight(pos);

                        if (height < MovementController.minimumReachDistance * 0.75)
                        {
                            pos.y = height;
                        }
                    }

                    Waypoint waypoint = GetOrAddWaypointComponent(m_target);
                    waypoint.Thing = thing;
                } 
                else
                {
                    m_target = null;
                }
            }
        }

        virtual internal Waypoint GetOrAddWaypointComponent(Transform target)
        {
            Waypoint waypoint = target.GetComponent<Waypoint>();
            if (!waypoint)
            {
                waypoint = target.gameObject.AddComponent<Waypoint>();
            }
            return waypoint;
        }

        public bool HasReachedTarget{
            get {
                if (m_targetReached) return true;
                else
                {
                    if (Vector3.Distance(transform.position, Target.position) <= MovementController.minimumReachDistance)
                    {
                        m_targetReached = true;
                        return true;
                    }
                }
                return false;
            }
            set
            {
                m_targetReached = value;
            }
        }

        new public AIMovementControllerSO MovementController
        {
            get { return (AIMovementControllerSO)_movementController; }
        }

        override internal void Update()
        {
            UpdateMove();
        }

        /// <summary>
        /// Typically the UpdateMove method is called from the Update method of the agent controller.
        /// It is responsible for making a decision about the agents next move by examining the state
        /// of the previous move (i.e. still moving towards target, at target, idle etc.) and, if 
        /// appropriate setting a new target.
        /// </summary>


        virtual internal void UpdateMove()
        {
            if (!Target)
            {
                Thing poi = GetPointOfInterest();
                if (poi)
                {
                    Target = poi.gameObject.transform;
                }
            }

            if (Target)
            {
                if (HasReachedTarget)
                {
                    ReachedTarget();
                }
            }
            else
            {
                UpdateWanderTarget();
            }
        }

        /// <summary>
        /// Update the WanderTarget position if it is time to do so.
        /// A new position for the target is chosen within a cone defined by the
        /// minAngleOfRandomPathChange and maxAngleOfRandomPathChange. Optionally,
        /// the cone can extend behind the current agent, which has the effect of 
        /// turning the agent around.
        /// </summary>
        internal void UpdateWanderTarget()
        {
            if (wanderTarget == null)
            {
                wanderTarget = ObjectPool.Instance.GetFromPool().transform;
            }

            timeToNextWanderPathChange -= Time.deltaTime;
            if (timeToNextWanderPathChange < 0)
            {
                Vector3 position = GetValidWanderPosition(transform, 0);

                wanderTarget.position = position;

                Target = wanderTarget;

                timeToNextWanderPathChange = Random.Range(MovementController.minTimeBetweenRandomPathChanges, MovementController.maxTimeBetweenRandomPathChanges);
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
                return home.transform.position;
            }

            Vector3 position;
            float minDistance = MovementController.minDistanceOfRandomPathChange;
            float maxDistance = MovementController.maxDistanceOfRandomPathChange;

            Quaternion randAng;
            if (!turnAround)
            {
                randAng = Quaternion.Euler(0, Random.Range(MovementController.minAngleOfRandomPathChange, MovementController.maxAngleOfRandomPathChange), 0);
            }
            else
            {
                randAng = Quaternion.Euler(0, Random.Range(180 - MovementController.minAngleOfRandomPathChange, 180 + MovementController.maxAngleOfRandomPathChange), 0);
                minDistance = maxDistance;
            }
            transform.rotation = transform.rotation * randAng;
            position = transform.position + randAng * Vector3.forward * Random.Range(minDistance, maxDistance);

            // calculate the new height 
            float terrainHeight = UnityEngine.Terrain.activeTerrain.SampleHeight(position);
            float newY = Mathf.Clamp(position.y, terrainHeight + MovementController.minimumFlyHeight, terrainHeight + MovementController.maximumFlyHeight);
            position.y = newY;

            if (attemptCount <= maxAttempts)
            {
                if (!IsValidWaypoint(position))
                {
                    position = GetValidWanderPosition(transform, attemptCount);
                }
                else
                {
                    return position;
                }
            }
            return Vector3.zero;
        }

        /// <summary>
        /// If the agent is tracking points of interest then select one and return it.
        /// If not tracking or no POI found return null.
        /// </summary>
        internal Thing GetPointOfInterest()
        {
            Thing poi = null;
            if (MovementController.seekPointsOfInterest)
            {
                // Look for points of interest
                if (Random.value <= 0.001)
                {
                    poi = FindPointOfInterest();
                }
            }
            return poi;
        }

        /// <summary>
        /// Get a point of interest for the agent to explore.
        /// </summary>
        /// <returns>Point of Interest to explore or null (if things of interest exist).</returns>
        Thing FindPointOfInterest()
        {
            Collider[] things = Physics.OverlapSphere(transform.position, detectionRange);
            if (things.Length > 0)
            {
                for (int i = 0; i < things.Length; i++)
                {
                    Thing thing = things[i].gameObject.GetComponent<Thing>();
                    if (thing != null)
                    {
                        if (!visitedThings.Contains(thing))
                        {
                            return thing;
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// View a Thing of Interest that is within range.
        /// </summary>
        internal virtual void ReachedTarget()
        {
            HasReachedTarget = true;

            Waypoint wp = Target.GetComponent<Waypoint>();
            Thing thing = wp.Thing;
            if (thing) // this is a Thing of interest
            {
                if (timeLeftLookingAtObject == float.NegativeInfinity)
                {
                    timeLeftLookingAtObject = thing.timeToLookAtObject;
                }

                timeLeftLookingAtObject -= Time.deltaTime;
                if (timeLeftLookingAtObject < 0)
                {
                    // Remember we have been here so we don't come again
                    visitedThings.Add(thing);

                    // we no longer care about this thing so turn the camera off and don't focus on it anymore
                    Target = null;
                    timeLeftLookingAtObject = float.NegativeInfinity;
                }
            }
            else // not a thing of interest
            {
                Target = null;
            }
        }

        /// <summary>
        /// Test to see if a given point is a valid waypoint for this agent.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        virtual internal bool IsValidWaypoint(Vector3 position)
        {
            Bounds bounds = UnityEngine.Terrain.activeTerrain.terrainData.bounds;
            return bounds.Contains(position);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Vector3 position = transform.position;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}
