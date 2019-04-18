using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.environment
{
    /// <summary>
    /// A Waypoint represents a destination for an agent.
    /// </summary>
    public class Waypoint : MonoBehaviour
    {
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
