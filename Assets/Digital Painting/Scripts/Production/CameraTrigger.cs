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
        [SerializeField]
        [Tooltip("Virtual Camera to use in this trigger zone. If the this collider is triggered currently in focus agent then the camera will switch to this one, with the LookAt set to the agent.")]
        private CinemachineVirtualCameraBase _virtualCamera;
        [SerializeField]
        [Tooltip("Configuration for this camera trigger.")]
        private CameraTriggerConfiguration config;
        [SerializeField]
        [Tooltip("Default look at target, only used if the lookAtTriggerAgent property is false.")]
        private Transform _defaultLookAtTarget = null;

        private DigitalPaintingManager _manager;

        private void Awake()
        {
            if (_virtualCamera == null)
            {
                _virtualCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
                if (_virtualCamera == null) { 
                    Debug.LogError("You have a `CameraTrigger` component attached to '" + gameObject.name + "' that does not have a Virtual Camera identified and the parent object does not have a Virtual Camera either. Disabling the trigger.");
                    this.enabled = false;
                }
            }

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
                Debug.LogWarning("You have a `CameraTrigger` component attached to '" + gameObject.name + "' but it does not have a trigger collider. One is required and thus the component has been disabled.");
                this.enabled = false;
            }

            _manager = FindObjectOfType<DigitalPaintingManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (config.AgentControllerWithFocus == null) return;

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
            if (config.AgentControllerWithFocus == null) return;

            if (GameObject.ReferenceEquals(other.gameObject, config.AgentControllerWithFocus.gameObject))
            {
                _virtualCamera.m_Priority -= config.PriorityBoost;
                if (config.OnEnterEvent != null) config.OnExitEvent.Raise();
            }
        }
    }
}
