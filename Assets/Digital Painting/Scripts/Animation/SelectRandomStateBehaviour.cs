using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsCode.Animation
{
    public class SelectRandomStateBehaviour : StateMachineBehaviour
    {
        [Tooltip("Parameter used to select the state to enter.")]
        public string StateSelectorParameter;
        [Tooltip("Number of states to select from")]
        public int NumberOfStates;

        // TODO: consider adding editor script to generate a script for deciding which state to play.
        // for example, see https://stackoverflow.com/questions/41709325/retrieve-all-animator-states-and-set-them-manually-in-code
        // this technique could be used to create a Scriptable object that contains all the states
        // and the weight for that state.
        override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            int idx = Random.Range(0, NumberOfStates);
            animator.SetInteger(StateSelectorParameter, idx);
        }
    }
}
