using UnityEngine;
using wizardscode.agent.movement;
using wizardscode.digitalpainting.agent;

namespace wizardscode.ai
{
    public class BaseMovementBrain : MonoBehaviour
    {
        [Tooltip("The movement controller that will manage movement for this agent.")]
        [SerializeField]
        internal MovementControllerSO _movementController;

        internal BaseAgentController agentController;

        public MovementControllerSO MovementController
        {
            get { return (AIMovementControllerSO)_movementController; }
            set { _movementController = value; }
        }

        internal virtual void UpdateMove()
        {

        }
    }
}
