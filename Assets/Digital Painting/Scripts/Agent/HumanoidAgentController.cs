using ScriptableObjectArchitecture;
using UnityEngine;
using WizardsCode.Agent.movement;
using WizardsCode.digitalpainting.agent;
using WizardsCode.editor;
using WizardsCode.validation;

namespace WizardsCode.Agent
{
    /// <summary>
    /// HuymanoidAgentController provides the core parameters and a very basic manual controller for Humanoid agents.
    /// 
    /// WASD provide forward/backward and strafe left/right
    /// QE provide up and down
    /// Right mouse button _ mouse provides look
    /// </summary>
    public class HumanoidAgentController : BaseAgentController
    {
        [Header("Starting Animation State")]
        [Tooltip("Check if you want the character to start the scene in the sitting position.")]
        public bool InitiallySitting = false;
        [Tooltip("Check if you want the character to start the scene talking position.")]
        public bool InitiallyTalking = false;

        internal override void Update()
        {
            if (isFirstFrame)
            {
                animator.SetBool(Settings.SittingAnimationParameter.Value, InitiallySitting);
                animator.SetBool(Settings.TalkingAnimationParameter.Value, InitiallyTalking);
                isFirstFrame = false;
            }    
        }
        
        /// <summary>
        /// Call this whenever the character starts to talk.
        /// </summary>
        public void StartTalking()
        {
            // TODO: use hash for animations
            animator.SetBool(Settings.TalkingAnimationParameter.Value, true);
        }
        
        /// <summary>
        /// Call this whenever the character stops talking.
        /// </summary>
        public void StopTalking()
        {
            // TODO: use hash for animations
            animator.SetBool(Settings.TalkingAnimationParameter.Value, false);
        }
    }
}
