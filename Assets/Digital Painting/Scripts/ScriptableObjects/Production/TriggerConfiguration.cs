using Cinemachine;
using ScriptableObjectArchitecture;
using UnityEngine;
using WizardsCode.Agent.movement;
using WizardsCode.digitalpainting.agent;

namespace WizardsCode.production
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
        private Director _director;

        private Director Director
        {
            get {
                if (!_director)
                {
                    _director = GameObject.FindObjectOfType<Director>();
                }
                return _director;
            }
        }

        public BaseAgentController AgentControllerWithFocus
        {
            get { return Director.AgentWithFocus; }
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