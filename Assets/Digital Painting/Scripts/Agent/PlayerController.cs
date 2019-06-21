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

        private Rigidbody m_RigidBody;
        private Animator m_Animator;
        private Camera m_Camera;
        internal float targetHeight = 0;
        internal float rotationX = 0;
        internal float rotationY = 0;
        internal bool isFlying = false;
        internal float timeUntilNextFlightTransitionPossible = 0;
        internal float debounceDelay = 0.25f;

        internal override void Awake()
        {
            base.Awake();
            m_Camera = Camera.main;
        }

        private void Start()
        {
            m_RigidBody = GetComponent<Rigidbody>();
            m_Animator = GetComponent<Animator>();
        }

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

            Vector3 move = GetMove();
            if (move != Vector3.zero)
            {
                if (move.magnitude > 1f)
                {
                    move.Normalize();
                }

                move = transform.InverseTransformDirection(move);

                RaycastHit hitInfo;
                Vector3 groundNormal = Vector3.up;
                if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, 0.1f))
                {
                    groundNormal = hitInfo.normal;
                }

                move = Vector3.ProjectOnPlane(move, groundNormal);
                float turnAmount = Mathf.Atan2(move.x, move.z);
                float forwardAmount = move.z;
                transform.position += transform.forward * Time.deltaTime * (forwardAmount * 4);

                // Rotation (in addition to any root motion rotation)
                float walkingRotationSpeed = MovementController.maxRotationSpeed * MovementController.normalMovementMultiplier;
                float turnSpeed = Mathf.Lerp(walkingRotationSpeed, MovementController.maxRotationSpeed, forwardAmount);
                transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
            }

            UpdateAnimator(move);
        }

        void UpdateAnimator(Vector3 move)
        {
            if (m_Animator == null)
            {
                return;
            }

            float turnAmount = Mathf.Atan2(move.x, move.z);
            float forwardSpeed = move.z;

            m_Animator.SetFloat(Settings.speedParameter, forwardSpeed, 0.1f, Time.deltaTime);
            m_Animator.SetFloat(Settings.turnParameter, turnAmount, 0.1f, Time.deltaTime);

            if (forwardSpeed != 0)
            {
                m_Animator.speed = forwardSpeed;
            }
            else
            {
                m_Animator.speed = 1;
            }
        }

        /// <summary>
        /// Typically the Move method is called from the Update method of the agent controller.
        /// It is responsible for making a decision about the agents next move and acting upon
        /// that decision.
        /// 
        /// In this instance it gets player input and calculates the next move.
        /// 
        /// <paramref name="transform">The transform of the agent to be moved.</paramref>
        /// </summary>
        private Vector3 GetMove()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            bool isCreeping = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

            Vector3 move = Vector3.zero;
            if (m_Camera != null)
            {
                Vector3 cameraForward = Vector3.Scale(m_Camera.transform.forward, new Vector3(1, 0, 1)).normalized;
                move = vertical * cameraForward + horizontal * m_Camera.transform.right;
            }
            else
            {
                move = vertical * Vector3.forward + horizontal * Vector3.right;
            }
            
            //move = vertical * transform.forward + horizontal * transform.right;

            if (isRunning)
            {
                move *= MovementController.fastMovementMultiplier;
            }
            else if (isCreeping)
            {
                move *= MovementController.slowMovementMultiplier;
            }
            else
            {
                move *= MovementController.normalMovementMultiplier;
            }

            if (MovementController.CanFly) 
            {
                bool takeOff = Input.GetKey(KeyCode.T);
                bool up = Input.GetKey(KeyCode.Q);
                bool down = Input.GetKey(KeyCode.E);

                timeUntilNextFlightTransitionPossible -= Time.deltaTime;
                if (timeUntilNextFlightTransitionPossible < 0 && takeOff)
                {
                    ToggleFlight();
                }

                if (isFlying)
                {
                    if (up)
                    {
                        targetHeight = targetHeight + (MovementController.climbSpeed * Time.deltaTime);
                    }

                    if (down)
                    {
                        targetHeight = targetHeight - (MovementController.climbSpeed * Time.deltaTime);
                    }
                }

                float heightDifference = targetHeight - transform.position.y;
                if (!Mathf.Approximately(targetHeight, 0))
                {
                    Vector3 pos = transform.position;
                    pos.y = pos.y + (heightDifference * Time.deltaTime);
                    transform.position = pos;

                    if (pos.y <= 0.1f && targetHeight <= 0.1f)
                    {
                        ToggleFlight();
                    }
                }
            }

            return move;
        }

        private void ToggleFlight()
        {
            isFlying = !isFlying;
            m_RigidBody.useGravity = !isFlying;
            if (isFlying)
            {
                targetHeight = 1.5f;
            }
            else
            {
                targetHeight = 0;
            }
            timeUntilNextFlightTransitionPossible = debounceDelay;
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
