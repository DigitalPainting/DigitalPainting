using Cinemachine;
using UnityEngine;
using wizardscode.agent.movement;
using wizardscode.digitalpainting.agent;

namespace wizardscode.production
{
    /// <summary>
    /// CameraTriggerConfiguration is a ScriptableObject that is used to define the behaviour of a camera when
    /// a collider trigger is entered or exited.
    /// </summary>
    [CreateAssetMenu(fileName = "TriggerConfiguration", menuName = "Wizards Code/Production/Trigger Configuration")]
    public class CameraTriggerConfiguration : TriggerConfiguration
    {
        [Header("Camera Configuration")]
        [SerializeField]
        [Tooltip("The base increase in camera priority when this trigger is fired")]
        [Range(1, 500)]
        private int _basePriorityBoost = 100;
        [SerializeField]
        [Tooltip("Should the camera follow the triggering agent?")]
        private bool _followTriggerAgent = false;
        [SerializeField]
        [Tooltip("Should the camera look at the triggering agent?")]
        private bool _lookAtTriggerAgent = true;

        public int PriorityBoost
        {
            get { return _basePriorityBoost; }
        }

        public bool FollowTriggerAgent
        {
            get { return _followTriggerAgent; }
        }

        public bool LookAtTriggerAgent
        {
            get { return _lookAtTriggerAgent; }
        }
    }
}