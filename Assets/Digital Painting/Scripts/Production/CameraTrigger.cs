using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.digitalpainting;

namespace wizardscode.production
{
    /// <summary>
    /// CameraTrigger should be attached to any object that has a trigger collider that is designed
    /// to influence the camera the Director chooses to activate. When something triggers the collider
    /// the weight for an affected camera will be changed and an event will be fired to inform the director
    /// of the change.
    /// </summary>
    public class CameraTrigger : MonoBehaviour
    {
        [Header("Camera Configuration", order = 100)]
        [SerializeField]
        [Tooltip("Virtual Camera to use in this trigger zone. If the this collider is triggered currently in focus agent then the camera will switch to this one, with the LookAt set to the agent. This can be either a prefab that will be instantiated and placed at a default location or it will be a reference to a camera pre-positioned in the scene.")]
        internal CinemachineVirtualCameraBase _virtualCamera;
        [SerializeField]
        [Tooltip("Configuration for this camera trigger.")]
        private CameraTriggerConfiguration config;
        [SerializeField]
        [Tooltip("Default look at target, only used if the lookAtTriggerAgent in the CameraTriggerConfig is false. " +
            "If this is null then the transform of the object this component is attached to will be used.")]
        private Transform _defaultLookAtTarget = null;

        private DigitalPaintingManager _manager;

        virtual protected void Awake()
        {
            bool hasTrigger = false;
            Collider[] colliders = GetComponents<Collider>();
            foreach (Collider collider in colliders)
            {
                if (collider.isTrigger)
                {
                    hasTrigger = true;
                    break;
                }
            }
            if (!hasTrigger)
            {
                float multiplier = 3f;
                Debug.LogWarning(name + " does not have a collider with trigger enabled, creating a default one that is twice the size of the Thing. Consider adding one that is an optimal size.");
                BoxCollider collider = gameObject.AddComponent<BoxCollider>();
                collider.isTrigger = true;
                Bounds bounds = GetChildRendererBounds(gameObject);
                collider.size = bounds.size * multiplier;
            }

            if (_defaultLookAtTarget == null)
            {
                _defaultLookAtTarget = transform;
            }

            _manager = FindObjectOfType<DigitalPaintingManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (config == null || config.AgentControllerWithFocus == null) return;

            if (GameObject.ReferenceEquals(other.gameObject, config.AgentControllerWithFocus.gameObject))
            {
                _virtualCamera.Priority += config.PriorityBoost;
                if (config.FollowTriggerAgent) _virtualCamera.Follow = other.gameObject.transform;
                if (config.LookAtTriggerAgent)
                {
                    _virtualCamera.LookAt = other.gameObject.transform;
                }
                else
                {
                    _virtualCamera.LookAt = _defaultLookAtTarget;
                }
                if (config.OnEnterEvent != null) config.OnEnterEvent.Raise();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (config == null || config.AgentControllerWithFocus == null) return;

            if (GameObject.ReferenceEquals(other.gameObject, config.AgentControllerWithFocus.gameObject))
            {
                _virtualCamera.m_Priority -= config.PriorityBoost;
                if (config.OnEnterEvent != null) config.OnExitEvent.Raise();
            }
        }

        protected Bounds GetChildRendererBounds(GameObject go)
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
