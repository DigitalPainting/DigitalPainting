using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.environment;

namespace wizardscode.ai
{
    /// <summary>
    /// The PointOfInterestBrain tracks known points of interest in the world and will 
    /// set a priority for the agent visiting one of those POIs.
    /// </summary>
    public class PointOfInterestBrain : MonoBehaviour
    {
        [Tooltip("The range the agent will use to detect things in its environment")]
        [SerializeField]
        internal float detectionRange = 25;

        private List<Thing> visitedThings = new List<Thing>();
        private float timeLeftLookingAtObject = float.NegativeInfinity;

        private BaseMovementBrain m_movementBrain;

        private void Awake()
        {
            m_movementBrain = GetComponent<BaseMovementBrain>();
            if (m_movementBrain == null)
            {
                Debug.LogError(gameObject.name + " has a PointOfInterestBrain but no MovementBrain with which to move to POIs.");
            }
        }

        /// <summary>
        /// Get or set the Point of Interest that currently has focus.
        /// If none have focus then return null.
        /// </summary>
        public Thing PointOfInterest { get; set; }

        /// <summary>
        /// Decides if there is a point of interest that the agent should focus on.
        /// </summary>
        internal void UpdatePointOfInterest()
        {
            // Look for points of interest
            if (PointOfInterest == null && Random.value <= 0.01)
            {
                PointOfInterest = FindPointOfInterest();
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

        internal void Update()
        {
            if (PointOfInterest == null)
            {
                UpdatePointOfInterest();
            }

            if (PointOfInterest != null) // Update POI doesn't always find something
            {
                m_movementBrain.Target = PointOfInterest.AgentViewingTransform;
                m_movementBrain.UpdateMove();

                if (m_movementBrain.HasReachedTarget())
                {
                    ViewPOI();
                }
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
    }
}
