using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using wizardscode.digitalpainting.agent;
using wizardscode.digitalpainting.environment;

public class InterestingThingsUI : MonoBehaviour {
    [Tooltip("The thing that the agent is currently interested in")]
    public Dropdown thingOfInterestDropdown;
    [Tooltip("Text object to display distance to current thing of interest.")]
    public Text distanceToThingOfInterestText;

    AIAgentController agent;
    List<Thing> things;

    private void Start()
    {
        agent = GameObject.FindObjectOfType<AIAgentController>();

        thingOfInterestDropdown.ClearOptions();
        things = new List<Thing>(GameObject.FindObjectsOfType<Thing>());
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        options.Add(new Dropdown.OptionData("Wander"));
        for (int i = 0; i < things.Count; i++)
        {
            options.Add(new Dropdown.OptionData(things[i].name));
        }
        thingOfInterestDropdown.AddOptions(options);
    }

    private void Update()
    {
        if (agent.thingOfInterest != null)
        {
            distanceToThingOfInterestText.text = "Distance: " + Vector3.Distance(agent.transform.position, agent.thingOfInterest.transform.position).ToString();
        } else
        {
            distanceToThingOfInterestText.text = "Wandering";
        }
    }

    private void LateUpdate()
    {
        if (agent.thingOfInterest != null)
        {
            thingOfInterestDropdown.value = things.FindIndex(x => x == agent.thingOfInterest) + 1;
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
            agent.thingOfInterest = things[thingOfInterestDropdown.value - 1];
        }
    }
}
