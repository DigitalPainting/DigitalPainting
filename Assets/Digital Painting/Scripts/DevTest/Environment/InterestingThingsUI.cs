using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using wizardscode.digitalpainting.agent;
using wizardscode.environment;

namespace wizardscode.devtest
{
    public class InterestingThingsUI : MonoBehaviour
    {
        [Header("User Interface)")]
        [Tooltip("The thing that the agent is currently interested in")]
        public Dropdown thingOfInterestDropdown;
        [Tooltip("Text object to display distance to current thing of interest.")]
        public Text distanceToThingOfInterestText;
        [Tooltip("List of addable things")]
        public Dropdown addableThingDropdown;
        [Tooltip("Interesting thing prefabs that can be added to the scene.")]
        public Thing[] addableThings;

        private ThingsManager thingsManager;
        AIAgentController agent;

        private void Start()
        {
            thingsManager = FindObjectOfType<ThingsManager>();
            agent = GameObject.FindObjectOfType<AIAgentController>();
            PopulateInterestingThingsDropdown();
            PopulateAddableThingsDropdown();
        }

        private void PopulateInterestingThingsDropdown()
        {
            thingOfInterestDropdown.ClearOptions();
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            options.Add(new Dropdown.OptionData("Wander"));
            for (int i = 0; i < thingsManager.allTheThings.Count; i++)
            {
                options.Add(new Dropdown.OptionData(thingsManager.allTheThings[i].name));
            }
            thingOfInterestDropdown.AddOptions(options);
        }

        private void PopulateAddableThingsDropdown()
        {
            addableThingDropdown.ClearOptions();
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            for (int i = 0; i < addableThings.Length; i++)
            {
                options.Add(new Dropdown.OptionData(addableThings[i].name));
            }
            addableThingDropdown.AddOptions(options);
        }

        private void Update()
        {
            PopulateInterestingThingsDropdown();

            if (agent.thingOfInterest != null)
            {
                distanceToThingOfInterestText.text = "Distance: " + Vector3.Distance(agent.transform.position, agent.thingOfInterest.transform.position).ToString();
            }
            else
            {
                distanceToThingOfInterestText.text = "Wandering";
            }
        }

        private void LateUpdate()
        {
            if (agent.thingOfInterest != null)
            {
                thingOfInterestDropdown.value = thingsManager.allTheThings.FindIndex(x => x == agent.thingOfInterest) + 1;
            }
            else
            {
                thingOfInterestDropdown.value = 0;
            }
        }

        public void OnThingSelectionChanged()
        {
            if (thingOfInterestDropdown.value == 0)
            {
                agent.thingOfInterest = null;
            }
            else
            {
                agent.thingOfInterest = thingsManager.allTheThings[thingOfInterestDropdown.value - 1];
            }
        }

        public void OnAddThingClicked()
        {
            Thing newThing = GameObject.Instantiate<Thing>(addableThings[0]);
            newThing.name = "This is " + transform.position.ToString();
            Vector3 position = agent.transform.position;
            position.y = Terrain.activeTerrain.SampleHeight(position) - 0.1f;
            position.x -= 0.1f;
            position.z -= 0.1f;
            newThing.transform.position = position;

            Vector3 lookAt = Camera.main.transform.position;
            newThing.transform.LookAt(new Vector3(lookAt.x, newThing.transform.position.y, lookAt.z));

            thingsManager.allTheThings.Add(newThing);
        }
    }
}
