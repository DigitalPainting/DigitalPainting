using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.digitalpainting.agent;

namespace wizardscode.validation
{
    [CreateAssetMenu(fileName = "DESCRIPTIVENAME_AgentSettingSO", menuName = "Wizards Code/Validation/Game Objects/Agent")]
    public class AgentSettingSO : PrefabSettingSO
    {
        public enum CameraAimMode { Composer, GroupComposer, HardLookAt, POV, SameAsFollowTarget }

        [Header("Camera Settings")]
        [Tooltip("The look at camera type")]
        public CameraAimMode cameraAimMode;

        internal override void InstantiatePrefab()
        {
            base.InstantiatePrefab();
            BaseAgentController controller = Instance.GetComponent<BaseAgentController>();
            controller.Settings = this;
        }
    }
}
