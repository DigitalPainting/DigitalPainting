using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.agent.movement;
using wizardscode.ai;
using wizardscode.environment;
using wizardscode.production;
using wizardscode.utility;

namespace wizardscode.digitalpainting.agent
{
    public class AIAgentController : BaseAgentController
    {   
        [Tooltip("The range the agent will use to detect things in its environment")]
        [SerializeField]
        internal float detectionRange = 50;

        [Header("Overrides")]
        [Tooltip("Set of objects within which the agent must stay. Each object must have a collider and non-kinematic rigid body. If null a default object will be searched for using the name `" + DEFAULT_BARRIERS_NAME + "`.")]
        public GameObject barriers;
        
        internal const string DEFAULT_BARRIERS_NAME = "AI Barriers";

        private List<Thing> visitedThings = new List<Thing>();
        private Thing _poi;
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

                Debug.LogWarning(gameObject.name + " is an AI Agent, but it did not have a collider that is not a trigger. One has been added automatically so that the agent will be contained by the '" + DEFAULT_BARRIERS_NAME + "'. Consider adding one.");
            }

            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                rigidbody = gameObject.AddComponent<Rigidbody>();
                rigidbody.useGravity = false;
                rigidbody.isKinematic = false;

                Debug.LogWarning(gameObject.name + " is an AI Agent, but it did not have rigidbody. One has been added automatically so that the agent will not be contained by the '" + DEFAULT_BARRIERS_NAME + "'. Consider adding one.");
            }
        }

        internal virtual void Start()
        {
            ConfigureBarriers();
        }

        public Thing PointOfInterest
        {
            get { return _poi; }
            set
            {
                _poi = value;
            }
        }

        internal void UpdatePointOfInterest()
        {
            if (movementBrain.SeeksPOI())
            {
                // Look for points of interest
                if (PointOfInterest == null && Random.value <= 0.001)
                {
                    PointOfInterest = FindPointOfInterest();
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
            if (PointOfInterest == null)
            {
                UpdatePointOfInterest();
            }

            if (PointOfInterest != null) // Update POI doesn't always find something
            {
                movementBrain.Target = PointOfInterest.AgentViewingTransform;
                movementBrain.UpdateMove();

                if (movementBrain.HasReachedTarget())
                {
                    ViewPOI();
                }
            }
            else
            {
                movementBrain.Target = null;
                movementBrain.UpdateMove();
            }
        }

        /// <summary>
        /// View a Thing of Interest that is within range.
        /// </summary>
        internal void ViewPOI()
        {
            if (timeLeftLookingAtObject == float.NegativeInfinity)
            {
                timeLeftLookingAtObject = PointOfInterest.timeToLookAtObject;
            }

            timeLeftLookingAtObject -= Time.deltaTime;
            if (timeLeftLookingAtObject < 0)
            {
                // Remember we have been here so we don't come again
                visitedThings.Add(PointOfInterest);

                // we no longer care about this thing so turn the camera off and don't focus on it anymore
                PointOfInterest = null;
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Vector3 position = transform.position;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}
