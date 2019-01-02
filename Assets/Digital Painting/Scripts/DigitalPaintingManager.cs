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

        private Cinemachine.CinemachineClearShot _clearshot;

        private BaseAgentController _agent;
        /// <summary>
        /// Get or set the agent that has the current focus of the camera.
        /// </summary>
        public BaseAgentController AgentWithFocus {
            get { return _agent; }
            set
            {
                _agent = value;
                _clearshot.Follow = _agent.transform;
                _clearshot.LookAt = _agent.transform;
            }
        }

        // Use this for initialization
        void Start()
        {
            CreateCamera();
            AgentWithFocus = CreateAgent();
        }

        /// <summary>
        /// Create the default camera rig.
        /// </summary>
        private void CreateCamera()
        {
            _clearshot = GameObject.Instantiate(cameraRigPrefab);
        }


        /// <summary>
        /// Create the main agent that the cameras will follow initially.
        /// </summary>
        /// <returns></returns>
        private BaseAgentController CreateAgent()
        {
            GameObject agent = GameObject.Instantiate(agentPrefab).gameObject;
            return agent.GetComponent<BaseAgentController>();
        }
    }
}
