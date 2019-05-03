using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.digitalpainting.agent;

namespace wizardscode.agent
{
    public class PlayerController : BaseAgentController
    {
        public enum MouseLookModeType { Never, Always, WithRightMouseButton }
        [Header("Manual Controls")]
        [Tooltip("Allow mouse look with no button")]
        public MouseLookModeType mouseLookMode = MouseLookModeType.WithRightMouseButton;
        [Tooltip("Mouse look sensitivity.")]
        public float mouseLookSensitivity = 100;

        internal float rotationX = 0;
        internal float rotationY = 0;

        override internal void Update()
        {
            // Mouse Look
            switch (mouseLookMode)
            {
                case MouseLookModeType.Always:
                    MouseLook();
                    break;
                case MouseLookModeType.WithRightMouseButton:
                    if (Input.GetMouseButton(1))
                    {
                        MouseLook();
                    }
                    break;
                default:
                    break;
            }

            Move();
        }

        /// <summary>
        /// Typically the Move method is called from the Update method of the agent controller.
        /// It is responsible for making a decision about the agents next move and acting upon
        /// that decision.
        /// <paramref name="transform">The transform of the agent to be moved.</paramref>
        /// </summary>
        private void Move()
        {
            // Move with the keyboard controls 
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                transform.position += transform.forward * (MovementController.normalMovementSpeed * MovementController.fastMovementFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += transform.right * (MovementController.normalMovementSpeed * MovementController.fastMovementFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                transform.position += transform.forward * (MovementController.normalMovementSpeed * MovementController.slowMovementFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += transform.right * (MovementController.normalMovementSpeed * MovementController.slowMovementFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
            }
            else
            {
                transform.position += transform.forward * MovementController.normalMovementSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += transform.right * MovementController.normalMovementSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.Q))
            {
                MovementController.heightOffset += MovementController.climbSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.E))
            {
                MovementController.heightOffset -= MovementController.climbSpeed * Time.deltaTime;
            }
        }

        private void MouseLook()
        {
            rotationX += Input.GetAxis("Mouse X") * mouseLookSensitivity * Time.deltaTime;
            rotationY += Input.GetAxis("Mouse Y") * mouseLookSensitivity * Time.deltaTime;
            rotationY = Mathf.Clamp(rotationY, -90, 90);
            transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
            transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
        }
    }
}
