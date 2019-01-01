using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.digitalpainting.agent;

namespace wizardscode.digitalpainting
{
    public class DigitalPaintingManager : MonoBehaviour
    {
        [Tooltip("The clear shot camera rig prefab to use")]
        public Cinemachine.CinemachineClearShot cameraRigPrefab;
        [Tooltip("The Agent prefab to use as the primary character - that is the one the camera will follow.")]
        public BaseAgentController agentPrefab;

        // Use this for initialization
        void Start()
        {
            GameObject agent = GameObject.Instantiate(agentPrefab).gameObject;
            Cinemachine.CinemachineClearShot clearshot = GameObject.Instantiate(cameraRigPrefab);
            clearshot.Follow = agent.transform;
            clearshot.LookAt = agent.transform;
        }
    }
}
