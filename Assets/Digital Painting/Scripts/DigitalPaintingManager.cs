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
            SetupBarriers();
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

        /// <summary>
        /// Create default barriers in the scene. These will be 10% in from the edge of the terrain borders on each side.
        /// </summary>
        private void SetupBarriers()
        {
            GameObject barriers = GameObject.Find(AIAgentController.DEFAULT_BARRIERS_NAME);
            if (barriers != null)
            {
                return;
            }

            Vector3 size = Terrain.activeTerrain.terrainData.size;
            float x = size.x;
            float y = size.y;
            float z = size.z;

            size.x = 2;

            // Parent
            barriers = new GameObject(AIAgentController.DEFAULT_BARRIERS_NAME);
            
            // Top
            GameObject barrier = GameObject.CreatePrimitive(PrimitiveType.Cube);
            barrier.transform.parent = barriers.transform;
            barrier.name = "Barrier 1";
            barrier.transform.localScale = size;
            barrier.transform.position = new Vector3(x * 0.1f, 0, z / 2);
            barrier.GetComponent<Renderer>().enabled = false;

            // Bottom
            barrier = GameObject.CreatePrimitive(PrimitiveType.Cube);
            barrier.transform.parent = barriers.transform;
            barrier.name = "Barrier 2";
            barrier.transform.localScale = size;
            barrier.transform.position = new Vector3(x * 0.9f, 0, z / 2);
            barrier.GetComponent<Renderer>().enabled = false;

            // Left
            barrier = GameObject.CreatePrimitive(PrimitiveType.Cube);
            barrier.transform.parent = barriers.transform;
            barrier.name = "Barrier 3";
            barrier.transform.localScale = size;
            barrier.transform.rotation = Quaternion.Euler(0, 90, 0);
            barrier.transform.position = new Vector3(x / 2, 0, z * 0.1f);
            barrier.GetComponent<Renderer>().enabled = false;

            // Right
            barrier = GameObject.CreatePrimitive(PrimitiveType.Cube);
            barrier.transform.parent = barriers.transform;
            barrier.name = "Barrier 4";
            barrier.transform.localScale = size;
            barrier.transform.rotation = Quaternion.Euler(0, 270, 0);
            barrier.transform.position = new Vector3(x / 2, 0, z * 0.9f);
            barrier.GetComponent<Renderer>().enabled = false;
        }
    }
}
