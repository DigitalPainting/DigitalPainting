using UnityEngine;
using wizardscode.digitalpainting.agent;

namespace wizardscode.agent
{
    [CreateAssetMenu(fileName = "Agent", menuName = "Wizards Code/Agent")]
    public class AgentScriptableObject : ScriptableObject
    {
        [Tooltip("The Agent prefab to use as the primary character - that is the one the camera will follow.")]
        public GameObject prefab;
        [Tooltip("Render agent - if this is set to off (unchecked) then the agent will not be visible.")]
        public bool render = true;

    }
}