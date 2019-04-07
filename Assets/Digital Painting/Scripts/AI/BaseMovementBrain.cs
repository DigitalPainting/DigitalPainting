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

        /// <summary>
        /// The position of the current waypoint, which is a point between the current location and the final Target.
        /// The last waypoint is the position of the Target itself.
        /// </summary>
        public virtual Vector3 WayPointPosition
        {
            get { return Target.position; }

        }

        protected Transform target;
        /// <summary>
        /// The current target that we are intended to move to.
        /// </summary>
        public Transform Target
        {
            get { return target; }
            set { target = value; }
        }

        private float m_Speed;
        public float Speed
        {
            get { return m_Speed; }
            set
            {
                if (value <= _movementController.maxSpeed)
                {
                    m_Speed = value;
                }
            }
        }

        public MovementControllerSO MovementController
        {
            get { return _movementController; }
            set { _movementController = value; }
        }

        internal virtual void UpdateMove()
        {

        }
    }
}
