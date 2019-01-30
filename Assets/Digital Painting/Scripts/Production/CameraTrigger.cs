using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.digitalpainting;

namespace wizardscode.production
{
    public class CameraTrigger : MonoBehaviour
    {
        [Header("Agent Interaction")]
        [Tooltip("Virtual Camera to use in this trigger zone. If the this collider is triggered currently in focus agent then the camera will switch to this one, with the LookAt set to the agent.")]
        public CinemachineVirtualCameraBase virtualCamera;

        private DigitalPaintingManager _manager;

        private void Awake()
        {
            if (virtualCamera == null)
            {
                Debug.LogError("You have a `CameraTrigger` component attached to '" + gameObject.name + "' that does not have a Virtual Camera identified. Disabling the trigger.");
                this.enabled = false;
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
            if (GameObject.ReferenceEquals(other.gameObject, _manager.AgentWithFocus.gameObject))
            {
                virtualCamera.enabled = true;
                virtualCamera.LookAt = other.gameObject.transform;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (GameObject.ReferenceEquals(other.gameObject, _manager.AgentWithFocus.gameObject))
            {
                virtualCamera.enabled = false;
            }
        }
    }
}
