using UnityEngine;
using WizardsCode.Agent.movement;
using WizardsCode.Editor;
using WizardsCode.Validation;

namespace WizardsCode.DigitalPainting.Agent
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
        [Expandable(isRequired: true)]
        [SerializeField]
        internal MovementControllerSO _movementController;

        [Tooltip("The settings for this agent that are used by the Digital Painting.")]
        [Expandable]
        public AgentSettingSO _Settings;

        [Header("Overrides")]
        [Tooltip("Home location of the agent. If blank this will be the agents starting position.")]
        public GameObject home;

        internal DigitalPaintingManager manager;
        internal Animator animator;
        internal bool isFirstFrame = true;

        private void Start()
        {
            animator = gameObject.GetComponent<Animator>();
        }

        internal virtual void Update()
        { 
        }
    
        /// <summary>
        /// When an agent is instantiated through the Digital Painting Manager in the Editor it
        /// will record the Settings for the agent. These can then be used by game engine at
        /// runtime. For example, special camera setups can be recorded here.
        /// </summary>
        public AgentSettingSO Settings
        {
            get
            {
                return _Settings;
            }
            set
            {
                _Settings = value;
            }
        }

        public MovementControllerSO MovementController
        {
            get { return _movementController; }
            internal set { _movementController = value; }
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
    }
}
