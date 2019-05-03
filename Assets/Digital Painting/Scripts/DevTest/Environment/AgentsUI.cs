using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using wizardscode.digitalpainting;
using wizardscode.digitalpainting.agent;
using wizardscode.production;

namespace wizardscode.devtest
{
    public class AgentsUI : MonoBehaviour
    {
        [Tooltip("The Agent that is currently focused")]
        public Dropdown agentDropdown;

        private DigitalPaintingManager manager;
        private BaseAgentController[] agents;
        private Director director;

        private void Awake()
        {
            director = GameObject.FindObjectOfType<Director>();
        }

        private void Start()
        {
            manager = GameObject.FindObjectOfType<DigitalPaintingManager>();
            agents = GameObject.FindObjectsOfType<BaseAgentController>();
            PopulateAgentsDropdown();
        }

        private void PopulateAgentsDropdown()
        {
            agentDropdown.ClearOptions();
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            for (int i = 0; i < agents.Length; i++)
            {
                options.Add(new Dropdown.OptionData(agents[i].name));
            }
            agentDropdown.AddOptions(options);
        }

        private void LateUpdate()
        {
            agentDropdown.value = Array.FindIndex(agents, x => x == director.AgentWithFocus);
        }

        public void OnAgentSelectionChanged()
        {
            director.AgentWithFocus = agents[agentDropdown.value];
        }
    }
}