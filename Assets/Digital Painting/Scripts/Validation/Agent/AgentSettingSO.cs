using UnityEngine;
using wizardscode.digitalpainting.agent;

namespace wizardscode.validation
{
    [CreateAssetMenu(fileName = "AgentSettingSO_DESCRIPTIVENAME", menuName = "Wizards Code/Validation/Game Objects/Agent")]
    public class AgentSettingSO : PrefabSettingSO
    {
        public enum CameraAimMode { Composer, GroupComposer, HardLookAt, POV, SameAsFollowTarget }

        [Header("Agent")]
        [Tooltip("The name of the game object for this UMA agent.")]
        public string agentName = "Agent";

        [Header("Animation Settings")]
        [Tooltip("The Speed parameter in the animation controller. This will be changed to reflect the current move speed of the character.")]
        public string speedParameter = "Speed";
        [Tooltip("The Direction parameter in the animation controller. This will be changed to reflect the current move direction of the character.")]
        public string turnParameter = "Direction";

        [Header("Camera Settings")]
        [Tooltip("Name of look at target in the prefab.")]
        public string lookAtName;
        [Tooltip("The look at camera type")]
        public CameraAimMode cameraAimMode;
        [Tooltip("Camera offset from agent.")]
        public Vector3 cameraFollowOffset = Vector3.zero;

        internal override void InstantiatePrefab()
        {
            base.InstantiatePrefab();
            BaseAgentController controller = Instance.GetComponent<BaseAgentController>();
            controller.Settings = this;
            Instance.name = agentName;

        }
    }
}
