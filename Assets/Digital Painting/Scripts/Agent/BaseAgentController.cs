﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.digitalpainting.agent
{
    /// <summary>
    /// BaseAgentController provides the core parameters and a very basic manual controller for agents.
    /// 
    /// WASD provide forward/backward and strafe left/right
    /// QE provide up and down
    /// Right mouse button _ mouse provides look
    /// </summary>
    public class BaseAgentController : MonoBehaviour
    {
        [Header("Movement")]
        [Tooltip("Walking speed under normal circumstances")]
        public float normalMovementSpeed = 1;
        [Tooltip("The factor by which to multiply the walking speed when moving fast.")]
        public float fastMovementFactor = 4;
        [Tooltip("The factor by which to multiply the walking speed when moving slowly.")]
        public float slowMovementFactor = 0.2f;
        [Tooltip("Speed at which the agent will climb/drop in flight. Set to 0 if you don't want them to fly.")]
        public float climbSpeed = 1;
        [Tooltip("The height above the terrain this agent should be.")]
        public float heightOffset = 0;
        [Tooltip("Speed at which the agent will rotate.")]
        public float rotationSpeed = 90;

        [Header("Manual Controls")]
        [Tooltip("Allow mouse look?")]
        public bool allowMouseLook = true;
        [Tooltip("Mouse look sensitivity.")]
        public float mouseLookSensitivity = 100;

        float rotationX = 0;
        float rotationY = 0;

        internal virtual void Update()
        {
            // Look with the mouse
            if (allowMouseLook)
            {
                rotationX += Input.GetAxis("Mouse X") * mouseLookSensitivity * Time.deltaTime;
                rotationY += Input.GetAxis("Mouse Y") * mouseLookSensitivity * Time.deltaTime;
                rotationY = Mathf.Clamp(rotationY, -90, 90);
                transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
                transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
            }

            // Move with the keyboard controls 
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                transform.position += transform.forward * (normalMovementSpeed * fastMovementFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += transform.right * (normalMovementSpeed * fastMovementFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                transform.position += transform.forward * (normalMovementSpeed * slowMovementFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += transform.right * (normalMovementSpeed * slowMovementFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
            }
            else
            {
                transform.position += transform.forward * normalMovementSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += transform.right * normalMovementSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.Q))
            {
                heightOffset += climbSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.E))
            {
                heightOffset -= climbSpeed * Time.deltaTime;
            }
        }
    }
}
