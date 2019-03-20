using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.agent.movement;
using wizardscode.environment;

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

            RobotMovementController pathfinding = GetComponent<RobotMovementController>();
            if (pathfinding == null)
            {
                Debug.LogWarning("No RobotMovementController found on " + gameObject.name + ". One has been added automatically, but consider adding one manually so that it may be optimally configured.");
                pathfinding = gameObject.AddComponent<RobotMovementController>();
            } else
            {
                ((FlyingAIMovementControllerSO)movementController).pathfinding = pathfinding;
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
                if (_thingOfInterest != null)
                {
                    manager.SetLookTarget(_thingOfInterest.transform);
                }
            }
        }
        internal void UpdatePointOfInterest()
        {
            // Look for points of interest
            if (ThingOfInterest == null && Random.value <= 0.001)
            {
                Thing poi = FindPointOfInterest();
                if (poi != null)
                {
                    ThingOfInterest = poi;
                    manager.SetLookTarget(ThingOfInterest.transform);
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
                movementController.target = ThingOfInterest.AgentViewingTransform;

                if (Vector3.Distance(transform.position, movementController.target.position) > ThingOfInterest.distanceToTriggerViewingCamera)
                {
                    movementController.Move(transform);
                }
                else
                {
                        ViewPOI();
                }
            }
            else
            {
                movementController.target = null;
                movementController.Move(transform);
            }
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

            CinemachineVirtualCamera virtualCamera = ThingOfInterest.virtualCamera;
            virtualCamera.enabled = true;

            timeLeftLookingAtObject -= Time.deltaTime;
            if (timeLeftLookingAtObject < 0)
            {
                // Remember we have been here so we don't come again
                visitedThings.Add(ThingOfInterest);

                // we no longer care about this thing so turn the camera off and don't focus on it anymore
                ThingOfInterest = null;
                timeLeftLookingAtObject = float.NegativeInfinity;
                virtualCamera.enabled = false;
            }
        }

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
