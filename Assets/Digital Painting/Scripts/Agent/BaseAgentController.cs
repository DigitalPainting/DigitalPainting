using Cinemachine;
using UnityEngine;
using wizardscode.agent.movement;
using wizardscode.ai;

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
        [Tooltip("Prefab for virtual camera to use when this is the agent with focus. If set to null the default camera for the scene will be used.")]
        [SerializeField]
        internal CinemachineVirtualCameraBase virtualCameraPrefab;

        public enum MouseLookModeType { Never, Always, WithRightMouseButton }
        [Header("Manual Controls")]
        [Tooltip("Allow mouse look with no button")]
        public MouseLookModeType mouseLookMode = MouseLookModeType.WithRightMouseButton;
        [Tooltip("Mouse look sensitivity.")]
        public float mouseLookSensitivity = 100;

        [Header("Overrides")]
        [Tooltip("Home location of the agent. If blank this will be the agents starting position.")]
        public GameObject home;

        private BaseMovementBrain _movementBrain;

        float rotationX = 0;
        float rotationY = 0;
        internal DigitalPaintingManager manager;
        internal Animator animator;

        internal MovementControllerSO MovementController
        {
            get { return MovementBrain.MovementController; }
        }

        internal BaseMovementBrain MovementBrain
        {
            get { return _movementBrain;  }
        }

        virtual internal void Awake()
        {
            manager = GameObject.FindObjectOfType<DigitalPaintingManager>();
            _movementBrain = FindObjectOfType<BaseMovementBrain>();
            if (MovementBrain == null)
            {
                Debug.LogError("There is no MovementBrain attached to " + gameObject.name);
            }

            animator = FindObjectOfType<Animator>();

            if (home == null)
            {
                home = new GameObject("Home for " + gameObject.name);
                home.transform.position = transform.position;
                home.transform.rotation = transform.rotation;
            }
        }

        virtual internal void Update()
        {
            // Mouse Look
            switch (mouseLookMode) {
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
        /// </summary>
        virtual public void Move()
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                MoveVerticalAxis(MovementBrain.MovementController.fastMovementFactor);
                MoveHorizontalAxis(MovementBrain.MovementController.fastMovementFactor);
            }
            else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                MoveVerticalAxis(MovementBrain.MovementController.slowMovementFactor);
                MoveHorizontalAxis(MovementBrain.MovementController.fastMovementFactor);
            }
            else
            {
                MoveVerticalAxis(1);
                MoveHorizontalAxis(1);
            }
        }

        /// <summary>
        /// Move the agent according to the Vertical Axis.
        /// </summary>
        /// <param name="speedMultiplier">A multiplier for the speed (e.g. run or crawl)</param>
        virtual internal void MoveVerticalAxis(float speedMultiplier)
        {
            if (!MovementBrain.MovementController.useRootMotion)
            {
                transform.position += transform.forward * (MovementBrain.MovementController.normalMovementSpeed * speedMultiplier) * Input.GetAxis("Vertical") * Time.deltaTime;
            }
        }

        /// <summary>
        /// Move the agent according to the Horizontal Axis.
        /// </summary>
        /// <param name="speedMultiplier">A multiplier for the speed (e.g. run or crawl)</param>
        virtual internal void MoveHorizontalAxis(float speedMultiplier)
        {
            if (!MovementBrain.MovementController.useRootMotion)
            {
                transform.position += transform.right * (MovementBrain.MovementController.normalMovementSpeed * speedMultiplier) * Input.GetAxis("Horizontal") * Time.deltaTime;
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
