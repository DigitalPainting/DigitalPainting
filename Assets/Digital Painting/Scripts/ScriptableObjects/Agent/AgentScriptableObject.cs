﻿using Cinemachine;
using UnityEngine;
using WizardsCode.Agent.movement;

namespace WizardsCode.Agent
{
    [CreateAssetMenu(fileName = "Agent", menuName = "Wizards Code/Agent/Agent Definition")]
    public class AgentScriptableObject : ScriptableObject
    {
        [Header("Model")]
        [Tooltip("The Agent prefab to use as the primary character - that is the one the camera will follow.")]
        public GameObject prefab;
        [Tooltip("Render agent - if this is set to off (unchecked) then the agent will not be visible.")]
        public bool render = true;
    }
}