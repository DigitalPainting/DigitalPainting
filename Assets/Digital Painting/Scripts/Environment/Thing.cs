using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.Production;

namespace WizardsCode.Environment
{
    /// <summary>
    /// This captures information about a thing in the world. 
    /// It is used to make objects that agents may take an 
    /// interest in.
    /// </summary>
    public class Thing : CameraTrigger
    {
        [Header("Positioning", order = 10)]
        [Tooltip("Should the object be grounded? If set to true the object will be placed on the ground when it is created.")]
        public bool isGrounded = true;
        [Tooltip("Y offset to be used when positioning the thing automatically.")]
        public float yOffset = 0;

        [Header("Viewing")]
        [Tooltip("Position and rotation the agent should adopt when viewing this thing. If null a location will be automatically created.")]
        public Transform _agentViewingTransform;
        [Tooltip("Time camera should spend paused looking at an object of interest when within range.")]
        public float timeToLookAtObject = 15;

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
                    float multiplier = 1.5f;
                    GameObject obj = new GameObject("Agent Viewing Position for " + gameObject.name);

                    Bounds bounds = this.GetComponent<Collider>().bounds;
                    Vector3 pos = new Vector3(bounds.center.x + bounds.extents.x * multiplier, bounds.center.y + bounds.extents.y * multiplier,  bounds.center.z + bounds.extents.z * multiplier);
                    
                    obj.transform.position = pos;
                    obj.transform.LookAt(transform.position);

                    AgentViewingTransform = obj.transform;
                }
                return _agentViewingTransform;
            }

            set
            {
                _agentViewingTransform = value;
            }
        }

        override protected void Awake()
        {
            base.Awake();

            if (isGrounded)
            {
                Vector3 position = gameObject.transform.position;
                position.y = UnityEngine.Terrain.activeTerrain.SampleHeight(position) + yOffset;
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

            if (_virtualCamera != null)
            {
                return;
            }

            Bounds bounds = this.GetComponent<Collider>().bounds;

            // TODO: Make a prefab and instantiate from that rather than creating at runtime
            GameObject camera = new GameObject();
            camera.name = "Virtual Camera for " + this.name;
            _virtualCamera = camera.AddComponent<CinemachineVirtualCamera>();
            _virtualCamera.m_StandbyUpdate = CinemachineVirtualCameraBase.StandbyUpdateMode.Never;
            _virtualCamera.LookAt = transform;
            _virtualCamera.Follow = transform;

            CinemachineFramingTransposer transposer = ((CinemachineVirtualCamera)_virtualCamera).AddCinemachineComponent<CinemachineFramingTransposer>();
            transposer.m_CameraDistance = bounds.extents.x * 2;

            ((CinemachineVirtualCamera)_virtualCamera).AddCinemachineComponent<CinemachineComposer>();

            camera.transform.parent = gameObject.transform;
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Vector3 position = transform.position;
            position.y = transform.position.y + transform.localScale.y;
            Gizmos.DrawIcon(position, "DigitalPainting/Thing.png", true);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Vector3 position = transform.position;
            position.y = transform.position.y + transform.localScale.y;
            Gizmos.DrawIcon(position, "DigitalPainting/Thing.png", true);
            if (_virtualCamera != null)
            {
                Gizmos.DrawIcon(transform.position, "DigitalPainting/VirtualCamera.png", true);
            }

            Gizmos.color = Color.green;
            if (_agentViewingTransform != null)
            {
                Gizmos.DrawLine(_agentViewingTransform.position, gameObject.transform.position);
            }

            Gizmos.color = Color.yellow;
            Bounds bounds = GetChildRendererBounds(gameObject);
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}
