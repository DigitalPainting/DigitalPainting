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
        [Header("Trigger Reaction")]
        [SerializeField]
        [Tooltip("Virtual Camera to use in this trigger zone. If the this collider is triggered currently in focus agent then the camera will switch to this one, with the LookAt set to the agent.")]
        private CinemachineVirtualCameraBase _virtualCamera;
        [SerializeField]
        [Tooltip("The base increase in camera priority when this trigger is fired")]
        [Range(1, 500)]
        private int basePriorityBoost = 100;
        [SerializeField]
        [Tooltip("Should the camera follow the triggering agent?")]
        private bool followTriggerAgent = false;
        [SerializeField]
        [Tooltip("Should the camera look at the triggering agent?")]
        private bool lookAtTriggerAgent = true;
        [SerializeField]
        [Tooltip("Default look at target, only used if the lookAtTriggerAgent property is false.")]
        private Transform defaultLookAtTarget = null;
        [SerializeField]
        [Tooltip("The GameEvent to fire when the collider is entered.")]
        private GameEvent _onEnterEvent = default(GameEvent);
        [SerializeField]
        [Tooltip("The GameEvent to fire when the collider is exited.")]
        private GameEvent _onExitEvent = default(GameEvent);

        [Header("System")]
        [SerializeField]
        [Tooltip("A reference to the agent that currently has focus.")]
        private BaseAgentControllerReference _agentWithFocus = default(BaseAgentControllerReference);

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
            if (_agentWithFocus.Value == null) return;

            if (GameObject.ReferenceEquals(other.gameObject, _agentWithFocus.Value.gameObject))
            {
                _virtualCamera.Priority += basePriorityBoost;
                if (followTriggerAgent) _virtualCamera.Follow = other.gameObject.transform;
                if (lookAtTriggerAgent)
                {
                    _virtualCamera.LookAt = other.gameObject.transform;
                }
                else
                {
                    _virtualCamera.LookAt = defaultLookAtTarget;
                }
                if (_onEnterEvent != null) _onEnterEvent.Raise();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_agentWithFocus.Value == null) return;

            if (GameObject.ReferenceEquals(other.gameObject, _agentWithFocus.Value.gameObject))
            {
                _virtualCamera.m_Priority -= basePriorityBoost;
                if (_onEnterEvent != null) _onExitEvent.Raise();
            }
        }
    }
}
