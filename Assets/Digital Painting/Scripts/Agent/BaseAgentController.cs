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
        [SerializeField]
        internal MovementControllerSO _movementController;

        [Header("Overrides")]
        [Tooltip("Home location of the agent. If blank this will be the agents starting position.")]
        public GameObject home;

        internal DigitalPaintingManager manager;

        public MovementControllerSO MovementController
        {
            get { return _movementController; }
        }

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

        virtual internal void Update() { }
    }
}
