using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsCode.Environment
{
    /// <summary>
    /// A Waypoint represents a destination for an agent.
    /// </summary>
    public class Waypoint : MonoBehaviour
    {
        [Tooltip("Is this the final destination or is this a point on the path to the final destination? If an interim point the agent need not get quite as close to this point to consider it reached, a rough approximation is sufficient.")]
        public bool finalDestination = true;
        private Thing m_thing;
        
        /// <summary>
        /// Set the current target to move to and/or interact with.
        /// </summary>
        /// <param name="thing">The Thing that the agent should move to and/or interact with.</param>
        public Thing Thing
        {
            get { return m_thing; }
            set
            {
                m_thing = value;
                if (value)
                {
                    transform.position = value.transform.position;
                }
            }
        }
    }
}
