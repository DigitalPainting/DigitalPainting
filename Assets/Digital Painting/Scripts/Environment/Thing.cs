using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.environment
{
    /// <summary>
    /// This captures information about a thing in the world. 
    /// It is used to make objects that agents may take an 
    /// interest in.
    /// </summary>
    public class Thing : MonoBehaviour
    {
        [Header("Viewing")]
        [Tooltip("Distance at which to switch to the viewing camera")]
        public float distanceToTriggerViewingCamera = 10;
        [Tooltip("Time camera should spend paused looking at an object of interest when within range.")]
        public float timeToLookAtObject = 15;
        [Tooltip("Virtual camera to use when viewing this thing. If null an attempt will be made to automatically place one in a sensible position.")]
        public CinemachineVirtualCamera virtualCamera;

        [SerializeField]
        private Guid _guid;
        public Guid Guid
        {
            get
            {
                if (_guid == null)
                {
                    _guid = new Guid();
                }
                return _guid;
            }
        }

        private void Awake()
        {
            if (GetComponent<Collider>() == null)
            {
                SphereCollider collider = gameObject.AddComponent<SphereCollider>();
                // TODO: if this object has a mesh then get bounds of the mesh and create a more accurate collider which is used for sizing
                // TODO: if this object has children make the collider encompass them too
                collider.radius = 0.3f;
            }
        }

        private void Start()
        {
            ConfigureVirtualCamera();
        }

        /// <summary>
        /// Add a virtual camera for viewing this object if there isn't one 
        /// already assigned to `virtualCamera`. If one is assigned prepare it
        /// for the scene. Called during startup.
        /// </summary>
        private void ConfigureVirtualCamera()
        {

            if (virtualCamera != null)
            {
                virtualCamera.enabled = false;
                return;
            }

            Bounds bounds = this.GetComponent<Collider>().bounds;

            // TODO: Make a prefab and instantiate from that rather than creating at runtime
            GameObject camera = new GameObject();
            camera.name = "Virtual Camera for " + this.name;
            virtualCamera = camera.AddComponent<CinemachineVirtualCamera>();
            virtualCamera.m_StandbyUpdate = CinemachineVirtualCameraBase.StandbyUpdateMode.Never;
            virtualCamera.LookAt = transform;
            virtualCamera.Follow = transform;

            CinemachineTransposer transposer = virtualCamera.AddCinemachineComponent<CinemachineTransposer>();
            transposer.m_FollowOffset.x = bounds.extents.x + (bounds.extents.x * 2);
            transposer.m_FollowOffset.y = bounds.extents.y + (bounds.extents.y * 2);
            transposer.m_FollowOffset.z = bounds.extents.z + (bounds.extents.z * 2);

            virtualCamera.AddCinemachineComponent<CinemachineComposer>();

            camera.transform.parent = gameObject.transform;

            virtualCamera.enabled = false;

            // TODO: verify the object is in full view of the camera, if not try a different position
        }
    }
}
