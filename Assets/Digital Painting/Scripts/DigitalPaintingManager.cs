using UnityEngine;
using WizardsCode.Agent;
using WizardsCode.digitalpainting.agent;
using WizardsCode.editor;
using WizardsCode.plugin;
using WizardsCode.production;
using Random = UnityEngine.Random;

namespace WizardsCode.digitalpainting
{
    public class DigitalPaintingManager : AbstractPluginManager
    {
        [Tooltip("The agents that exist in the world. These agents will act autonomously in the world, doing interesting things. The first agent in the list will be the first one in the list is the one that the camera will initially be viewing.")]
        public AgentScriptableObject[] agentObjectDefs;
        
        private Director director;

        void Awake()
        {
            director = GameObject.FindObjectOfType<Director>();
        }

        private void Start()
        {
            for (int i = 0; i < agentObjectDefs.Length; i++)
            {
                BaseAgentController agent = CreateAgent("Agent: " + i + " " + agentObjectDefs[i].prefab.name, agentObjectDefs[i]);
                if (i == 0)
                {
                    director.AgentWithFocus = agent;
                }
            }
        }

        /// <summary>
        /// Create an agent.
        /// </summary>
        /// <returns></returns>
        private BaseAgentController CreateAgent(string name, AgentScriptableObject def)
        {
            GameObject agent = GameObject.Instantiate(def.prefab).gameObject;
            agent.name = name;
            BaseAgentController controller = agent.GetComponent<BaseAgentController>();

            Renderer renderer = agent.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.enabled = def.render;
            }

            Vector3 position = GetSpawnPositionCandidate(controller);
            agent.transform.position = position;

            return controller;
        }

        private static Vector3 GetSpawnPositionCandidate(BaseAgentController controller)
        {
            float border = Terrain.activeTerrain.terrainData.size.x / 10;
            float x = Random.Range(border, Terrain.activeTerrain.terrainData.size.x - border);
            float z = Random.Range(border, Terrain.activeTerrain.terrainData.size.z - border);
            Vector3 position = new Vector3(x, 0, z);

            float y = Terrain.activeTerrain.SampleHeight(position);
            position.y = y + controller.MovementController.heightOffset;
            return position;
        }
    }
}
