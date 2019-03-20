using UnityEngine;
using wizardscode.agent.movement;

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
        [Tooltip("The movement controller that will manage movement for this agent.")]
        public MovementControllerSO movementController;

        public enum MouseLookModeType { Never, Always, WithRightMouseButton }
        [Header("Manual Controls")]
        [Tooltip("Allow mouse look with no button")]
        public MouseLookModeType mouseLookMode = MouseLookModeType.WithRightMouseButton;
        [Tooltip("Mouse look sensitivity.")]
        public float mouseLookSensitivity = 100;

        [Header("Overrides")]
        [Tooltip("Home location of the agent. If blank this will be the agents starting position.")]
        public GameObject home;

        float rotationX = 0;
        float rotationY = 0;
        internal DigitalPaintingManager manager;

        virtual internal void Awake()
        {
            manager = GameObject.FindObjectOfType<DigitalPaintingManager>();

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

            movementController.Move(transform);
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
