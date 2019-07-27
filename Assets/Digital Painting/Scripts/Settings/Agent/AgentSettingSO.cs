using ScriptableObjectArchitecture;
using UnityEngine;

using WizardsCode.DigitalPainting.Agent;

namespace WizardsCode.Validation
{
    /// <summary>
    /// The AgentSettignSO class describes an agent in the game. This is a reasonably generic description,
    /// specific agent types might subclass this to provide more details that are exclusive to that type.
    /// </summary>
    [CreateAssetMenu(fileName = "DESCRIPTIVENAME_AgentSettingSO_Default", menuName = "Wizards Code/Digital Painting/Settings/Agent")]
    public class AgentSettingSO : PrefabSettingSO
    {
        public enum CameraAimMode { Composer, GroupComposer, HardLookAt, POV, SameAsFollowTarget }

        [Header("Agent")]
        [Tooltip("The name of the game object for this UMA agent.")]
        public string agentName = "Agent";

        [Header("Animation Parameters")]
        [Tooltip("The name of a boolean value that is used to play the Talking animations.")]
        public StringReference TalkingAnimationParameter = new StringReference("Talking");

        [Tooltip("The name of a boolean parameter that is used to indicate if the character is sitting down.")]
        public StringReference SittingAnimationParameter = new StringReference("Sitting");

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
            if (controller) {
              controller.Settings = this;
            }
            Instance.name = agentName;

            PositionOnGround(Instance.transform.position);
        }

        internal void PositionOnGround(Vector3 pos)
        {
            float terrainHeight = UnityEngine.Terrain.activeTerrain.SampleHeight(pos);
            pos.y = terrainHeight;
            Instance.transform.position = pos;
        }
    }
}
