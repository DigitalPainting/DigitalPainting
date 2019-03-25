using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.agent.movement;
using wizardscode.environment;
using wizardscode.production;
using wizardscode.utility;

namespace wizardscode.digitalpainting.agent
{
    public class AIAgentController : BaseAgentController
    {
        [Tooltip("The range the agent will use to detect things in its environment")]
        public float detectionRange = 50;

        [Header("Overrides")]
        [Tooltip("Set of objects within which the agent must stay. Each object must have a collider and non-kinematic rigid body. If null a default object will be searched for using the name `" + DEFAULT_BARRIERS_NAME + "`.")]
        public GameObject barriers;
        
        internal const string DEFAULT_BARRIERS_NAME = "AI Barriers";

        private List<Thing> visitedThings = new List<Thing>();
        private Thing _thingOfInterest;
        private float timeLeftLookingAtObject = float.NegativeInfinity;
        internal float timeToNextWanderPathChange = 0;
        
        private Transform target; // the current target that we are moving towards
        private RobotMovementController pathfinding;
        private Transform wanderTarget;

        new public AIMovementControllerSO MovementController
        {
            get { return (AIMovementControllerSO)_movementController; }
        }

        override internal void Awake()
        {
            base.Awake();

            bool hasCollider = false;
            Collider[] colliders = gameObject.GetComponents<Collider>();
            for (int i = 0; i < colliders.Length; i++)
            {
                if (!colliders[i].isTrigger)
                {
                    hasCollider = true;
                    break;
                }
            }
            if (!hasCollider)
            {
                Collider collider = gameObject.AddComponent<SphereCollider>();
                collider.isTrigger = false;

                Debug.LogWarning(gameObject.name + " is an AI Agent, but it did not have a collider that is not a trigger. One has been added automatically so that the agent will not be contained by the '" + DEFAULT_BARRIERS_NAME + "'. Consider adding one.");
            }

            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                rigidbody = gameObject.AddComponent<Rigidbody>();
                rigidbody.useGravity = false;
                rigidbody.isKinematic = false;

                Debug.LogWarning(gameObject.name + " is an AI Agent, but it did not have rigidbody. One has been added automatically so that the agent will not be contained by the '" + DEFAULT_BARRIERS_NAME + "'. Consider adding one.");
            }

            pathfinding = GetComponent<RobotMovementController>();
            if (pathfinding == null)
            {
                Debug.LogWarning("No RobotMovementController found on " + gameObject.name + ". One has been added automatically, but consider adding one manually so that it may be optimally configured.");
                pathfinding = gameObject.AddComponent<RobotMovementController>();
            }
        }

        internal void Start()
        {
            ConfigureBarriers();
        }

        public Thing ThingOfInterest
        {
            get { return _thingOfInterest; }
            set
            {
                _thingOfInterest = value;
            }
        }

        internal void UpdatePointOfInterest()
        {
            if (MovementController.seekPointsOfInterest)
            {
                // Look for points of interest
                if (ThingOfInterest == null && Random.value <= 0.001)
                {
                    Thing poi = FindPointOfInterest();
                }
            }
        }

        /// <summary>
        /// Barriers are a group of colliders that are used to keep agents within a defined area.
        /// </summary>
        private void ConfigureBarriers()
        {
            if (barriers == null)
            {
                GameObject barriers = GameObject.Find(DEFAULT_BARRIERS_NAME);
                if (barriers == null)
                {
                    Debug.LogError("No `"+ DEFAULT_BARRIERS_NAME + "` to contain the AI Agents found. Create an empty object with children that enclose the area your AI should move within (the children need non-kinematic rigid bodies and colliders). If you If you call it `" + DEFAULT_BARRIERS_NAME + "` then the agent will automatically pick it up, if you need to use a different name drag it into the `Barriers` field of the controller component on your agent.");
                }
            }
        }

        override internal void Update()
        {
            if (ThingOfInterest == null)
            {
                UpdatePointOfInterest();
            }

            if (ThingOfInterest != null) // Update POI doesn't always find something
            {
                target = ThingOfInterest.AgentViewingTransform;

                if (Vector3.Distance(transform.position, target.position) > ThingOfInterest.distanceToTriggerViewingCamera)
                {
                    Move();
                }
                else
                {
                    ViewPOI();
                }
            }
            else
            {
                target = null;
                Move();
            }
        }

        /// <summary>
        /// Typically the Move method is called from the Update method of the agent controller.
        /// It is responsible for making a decision about the agents next move and acting upon
        /// that decision.
        /// </summary>
        private void Move()
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
                UpdateWanderTarget();

                if (Vector3.Distance(transform.position, wanderTarget.position) <= pathfinding.minReachDistance)
                {
                    UpdateWanderTarget();
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
            if (wanderTarget == null)
            {
                wanderTarget = ObjectPool.Instance.GetFromPool().transform;
            }

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
                return Vector3.zero;
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

        /// <summary>
        /// View a Thing of Interest that is within range.
        /// </summary>
        internal void ViewPOI()
        {
            if (timeLeftLookingAtObject == float.NegativeInfinity)
            {
                timeLeftLookingAtObject = ThingOfInterest.timeToLookAtObject;
            }

            timeLeftLookingAtObject -= Time.deltaTime;
            if (timeLeftLookingAtObject < 0)
            {
                // Remember we have been here so we don't come again
                visitedThings.Add(ThingOfInterest);

                // we no longer care about this thing so turn the camera off and don't focus on it anymore
                ThingOfInterest.virtualCamera.enabled = false;
                ThingOfInterest = null;
                timeLeftLookingAtObject = float.NegativeInfinity;
            }
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
    }
}
