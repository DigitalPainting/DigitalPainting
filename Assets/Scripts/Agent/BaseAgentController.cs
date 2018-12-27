using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.agent
{
    public class BaseAgentController : MonoBehaviour
    {
        [Header("Movement")]
        [Tooltip("Walking speed under normal circumstances")]
        public float NormalMovementSpeed = 6;
        [Tooltip("The factor by which to multiply the walking speed when moving fast.")]
        public float FastMovementFactor = 4;
        [Tooltip("The factor by which to multiply the walking speed when moving slowly.")]
        public float SlowMovementFactor = 0.2f;
        [Tooltip("Speed at which the agent will climb/drop in flight. Set to 0 if you don't want them to fly.")]
        public float ClimbSpeed = 4;
        [Tooltip("Speed at which the agent will rotate.")]
        public float RotationSpeed = 90;

        [Header("Controls")]
        public float rotationSensitivity = 100;

        float rotationX = 0;
        float rotationY = 0;


        void Update()
        {
            // Look with the mouse
            rotationX += Input.GetAxis("Mouse X") * rotationSensitivity * Time.deltaTime;
            rotationY += Input.GetAxis("Mouse Y") * rotationSensitivity * Time.deltaTime;
            rotationY = Mathf.Clamp(rotationY, -90, 90);
            transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
            transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

            // Move with the keyboard controls 
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                transform.position += transform.forward * (NormalMovementSpeed * FastMovementFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += transform.right * (NormalMovementSpeed * FastMovementFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                transform.position += transform.forward * (NormalMovementSpeed * SlowMovementFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += transform.right * (NormalMovementSpeed * SlowMovementFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
            }
            else
            {
                transform.position += transform.forward * NormalMovementSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += transform.right * NormalMovementSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.Q))
            {
                transform.position += transform.up * ClimbSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.E))
            {
                transform.position -= transform.up * ClimbSpeed * Time.deltaTime;
            }
        }
    }
}
