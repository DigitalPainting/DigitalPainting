using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.environment;

namespace wizardscode.agent
{
    public class AIAgentController : BaseAgentController
    {
        [Header("AI Controller")]
        [Tooltip("Is the agent automated or manual movement?")]
        public bool isAutomatedMovement = true;

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
        [Tooltip("Collider within which the drone must stay. If null an object called 'SafeArea' is used.")]
        public Collider safeAreaCollider;

        internal Quaternion targetRotation;
        private float timeToNextPathChange = 3;
        private float timeLeftLookingAtObject;
        private List<Thing> visitedThings = new List<Thing>();

        private void Awake()
        {
            if (safeAreaCollider == null)
            {
                safeAreaCollider = GameObject.Find("SafeArea").GetComponent<Collider>();
            }
        }
        
        internal override void Update()
        {
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            if (!isAutomatedMovement)
            {
                base.Update();
            }
            else
            {

                if (!safeAreaCollider.bounds.Contains(transform.position))
                {
                    targetRotation = Quaternion.LookRotation(safeAreaCollider.bounds.center - transform.position, Vector3.up);
                }
                else
                {
                    MakeNextMove();
                }
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
                    visitedThings.Add(thingOfInterest);
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
    }
}
