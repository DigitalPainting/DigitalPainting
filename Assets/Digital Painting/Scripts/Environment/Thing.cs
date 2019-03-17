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
        [Header("Positioning")]
        [Tooltip("Should the object be grounded? If set to true the object will be placed on the ground when it is created.")]
        public bool isGrounded = true;
        [Tooltip("Y offset to be used when positioning the thing automatically.")]
        public float yOffset = 0;

        [Header("Viewing")]
        [Tooltip("Position and rotation the agent should adopt when viewing this thing. If null a location will be automatically created.")]
        public Transform _agentViewingTransform;
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

        /// <summary>
        /// Get the viewing position for this thing of interest.
        /// </summary>
        public Transform AgentViewingTransform
        {
            get {
                if (_agentViewingTransform == null)
                {
                    GameObject obj = new GameObject("Agent Viewing Position for " + gameObject.name);

                    Bounds bounds = this.GetComponent<Collider>().bounds;
                    Vector3 pos = new Vector3(bounds.center.x + bounds.extents.x * 2, 0,  bounds.center.z + bounds.extents.z * 2);
                    pos.y = Terrain.activeTerrain.SampleHeight(pos) + bounds.extents.y * 2;
                    obj.transform.position = pos;
                    obj.transform.LookAt(transform.position);

                    _agentViewingTransform = obj.transform;
                }
                return _agentViewingTransform;
            }
        }

        private void Awake()
        {
            if (GetComponent<Collider>() == null)
            {
                BoxCollider collider = gameObject.AddComponent<BoxCollider>();
                collider.isTrigger = true;
                Bounds bounds = GetChildRendererBounds(gameObject);
                collider.size = bounds.size;
            }

            if (isGrounded)
            {
                Vector3 position = gameObject.transform.position;
                position.y = Terrain.activeTerrain.SampleHeight(position) + yOffset;
                gameObject.transform.position = position;
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Vector3 position = transform.position;
            position.y = transform.position.y + transform.localScale.y;
            Gizmos.DrawIcon(position, "DigitalPainting/Thing.png", true);
            if (virtualCamera != null)
            {
                Gizmos.DrawIcon(transform.position, "DigitalPainting/VirtualCamera.png", true);
            }

            Bounds bounds = GetChildRendererBounds(gameObject);
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }

        Bounds GetChildRendererBounds(GameObject go)
        {
            Renderer[] renderers = go.GetComponentsInChildren<Renderer>();

            if (renderers.Length > 0)
            {
                Bounds bounds = renderers[0].bounds;
                for (int i = 1, ni = renderers.Length; i < ni; i++)
                {
                    bounds.Encapsulate(renderers[i].bounds);
                }
                return bounds;
            }
            else
            {
                return new Bounds();
            }
        }
    }
}
