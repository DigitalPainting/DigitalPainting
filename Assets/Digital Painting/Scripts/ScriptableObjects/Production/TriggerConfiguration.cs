using Cinemachine;
using UnityEngine;
using wizardscode.agent.movement;
using wizardscode.digitalpainting.agent;

namespace wizardscode.production
{
    /// <summary>
    /// TriggerConfiguration enables GameEvents to be fired in response to a Collider being Triggered. It
    /// also allows the configuration of what agents will trigger the effects of this trigger.
    /// </summary>
    [CreateAssetMenu(fileName = "TriggerConfiguration", menuName = "Wizards Code/Production/Trigger Configuration")]
    public class TriggerConfiguration : ScriptableObject
    {
        [Header("Trigger Events")]
        [SerializeField]
        [Tooltip("The GameEvent to fire when the collider is entered.")]
        private GameEvent _onEnterEvent = default(GameEvent);
        [SerializeField]
        [Tooltip("The GameEvent to fire when the collider is exited.")]
        private GameEvent _onExitEvent = default(GameEvent);

        [Header("Variable References")]
        [SerializeField]
        [Tooltip("A reference to the agent that currently has focus.")]
        private BaseAgentControllerReference _agentWithFocus = default(BaseAgentControllerReference);

        public BaseAgentController AgentControllerWithFocus
        {
            get { return _agentWithFocus.Value; }
        }
        
        public GameEvent OnEnterEvent
        {
            get { return _onEnterEvent; }
        }

        public GameEvent OnExitEvent
        {
            get { return _onExitEvent; }
        }
    }
}