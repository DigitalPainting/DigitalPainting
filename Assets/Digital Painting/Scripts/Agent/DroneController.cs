using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.agent
{
    /// <summary>
    /// DroneController is a FlyByWire or manual controller for a drone.
    /// 
    /// When using FlyByWire it flies randomly around a scene, but will 
    /// never stay outside of the bounding box of the 'safeAreaCollider'.
    /// </summary>
    public class DroneController : BaseAgentController
    {
        [Header("Fly By Wire Controls")]
        [Tooltip("Is the drone automated or manual?")]
        public bool isFlyByWire = true;
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

        private Quaternion targetRotation;
        private float timeToNextPathChange = 3;

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
            if (!isFlyByWire)
            {
                base.Update();
            }
            else
            {
                if (position == transform.position && rotation == transform.rotation)
                {
                    FlyByWire();
                }
            }
        }

        /// <summary>
        /// Moves the drone according to the fly by wire program being carried.
        /// </summary>
        private void FlyByWire()
        {
            if (!safeAreaCollider.bounds.Contains(transform.position))
            {
                targetRotation = Quaternion.LookRotation(safeAreaCollider.bounds.center - transform.position, Vector3.up);
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

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            transform.position += transform.forward * normalMovementSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            SetHeight();
        }

        /// <summary>
        /// Set the current height of the drone based on the terrain height.
        /// </summary>
        private void SetHeight()
        {
            // get the current position and height above the terrain
            Vector3 position = transform.position;
            
            // calculate the new position and height 
            position.y = Terrain.activeTerrain.SampleHeight(position) + heightOffset;

            // reposition the camera 
            transform.position = position;
        }
    }
}
