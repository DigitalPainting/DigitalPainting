using UnityEngine;
using WizardsCode.Agent;
using WizardsCode.DigitalPainting.Agent;
using WizardsCode.Plugin;
using WizardsCode.Production;
using Random = UnityEngine.Random;

namespace WizardsCode.DigitalPainting
{
    public class DigitalPaintingManager : AbstractPluginManager
    {
        private Director director;

        void Awake()
        {
            director = GameObject.FindObjectOfType<Director>();
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
            float border = UnityEngine.Terrain.activeTerrain.terrainData.size.x / 10;
            float x = Random.Range(border, UnityEngine.Terrain.activeTerrain.terrainData.size.x - border);
            float z = Random.Range(border, UnityEngine.Terrain.activeTerrain.terrainData.size.z - border);
            Vector3 position = new Vector3(x, 0, z);

            float y = UnityEngine.Terrain.activeTerrain.SampleHeight(position);
            position.y = y + controller.MovementController.heightOffset;
            return position;
        }
    }
}
