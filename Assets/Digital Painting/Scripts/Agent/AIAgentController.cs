using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.environment;

namespace wizardscode.digitalpainting.agent
{
    public class AIAgentController : BaseAgentController
    {
        [Header("AI Controller")]
        [Tooltip("Is the agent automated or manual movement?")]
        public bool isFlyByWire = true;

        [Header("Objects of Interest")]
        [Tooltip("Current thing of interest. The agent will move to and around the object until it is no longer interested, then it will make this parameter null. When null the agent will move according to other algorithms.")]
        public Thing thingOfInterest;
        [Tooltip("The range the agent will use to detect things in its environment")]
        public float detectionRange = 50;

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

        [Header("Overrides")]
        [Tooltip("Set of objects within which the agent must stay. Each object must have a collider and non-kinematic rigid body. If null a default object will be searched for using the name `" + DEFAULT_BARRIERS_NAME + "`.")]
        public GameObject barriers;

        internal const string DEFAULT_BARRIERS_NAME = "AI Barriers";

        internal Quaternion targetRotation;
        private float timeToNextPathChange = 3;
        private float timeLeftLookingAtObject;
        private List<Thing> visitedThings = new List<Thing>();

        override internal void Awake()
        {
            base.Awake();
            ConfigureBarriers();

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
                Debug.LogWarning(gameObject.name + " is an AI Agent, but it does not have a collider that is not a trigger. This means the agent will not be contained contained by the '" + DEFAULT_BARRIERS_NAME + "'");
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

        internal override void Update()
        {
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            if (!isFlyByWire)
            {
                base.Update();
            }
            else
            {
                MakeNextMove();
            }

            // Look for points of interest
            if (thingOfInterest == null && Random.value <= 0.001)
            {
                Thing poi = FindPointOfInterest();
                if (poi != null)
                {
                    thingOfInterest = poi;
                }
            }
        }

        /// <summary>
        /// Sets up the state of the agent such that the next movement can be made by changing position and rotation of the agent
        /// </summary>
        internal void MakeNextMove()
        {
            if (thingOfInterest != null)
            {
                targetRotation = Quaternion.LookRotation(thingOfInterest.transform.position - transform.position, Vector3.up);
            }
            else
            {
                // add some randomness to the flight 
                timeToNextPathChange -= Time.deltaTime;
                if (timeToNextPathChange <= 0)
                {
                    float rotation = Random.Range(minAngleOfRandomPathChange, maxAngleOfRandomPathChange);
                    Vector3 newRotation = targetRotation.eulerAngles;
                    newRotation.y += rotation;
                    targetRotation = Quaternion.Euler(newRotation);
                    timeToNextPathChange = Random.Range(minTimeBetweenRandomPathChanges, maxTimeBetweenRandomPathChanges);
                }
            }

            Vector3 position = transform.position;
            if (thingOfInterest != null && Vector3.Distance(position, thingOfInterest.transform.position) > thingOfInterest.distanceToTriggerViewingCamera)
            {
                position += transform.forward * normalMovementSpeed * Time.deltaTime;
                position.y -= (position.y - thingOfInterest.transform.position.y) * climbSpeed * Time.deltaTime;

                timeLeftLookingAtObject = thingOfInterest.timeToLookAtObject;
            }
            else if (thingOfInterest != null)
            {
                CinemachineVirtualCamera virtualCamera = thingOfInterest.virtualCamera;
                virtualCamera.enabled = true;

                timeLeftLookingAtObject -= Time.deltaTime;
                if (timeLeftLookingAtObject < 0)
                {
                    // Remember we have been here so we don't come again
                    visitedThings.Add(thingOfInterest);

                    // when we start moving again move away from the object as we are pretty close by now and might move into it
                    targetRotation = Quaternion.LookRotation(-transform.forward, Vector3.up);

                    // we no longer care about this thing so turn the camera off and don't focus on it anymore
                    thingOfInterest = null;
                    virtualCamera.enabled = false;
                }
            }
            else
            {
                // calculate the new position and height
                position += transform.forward * normalMovementSpeed * Time.deltaTime;
                float desiredHeight = Terrain.activeTerrain.SampleHeight(position) + heightOffset;
                position.y += (desiredHeight - position.y) * Time.deltaTime;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.position = position;
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

        void OnCollisionEnter(Collision collision)
        {
            targetRotation = Quaternion.LookRotation(home.transform.position - transform.position, Vector3.up);
        }
    }
}
