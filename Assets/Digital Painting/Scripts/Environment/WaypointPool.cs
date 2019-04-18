using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.utility;

namespace wizardscode.environment
{
    public class WaypointPool : ObjectPool
    {
        private void OnDisable()
        {
            Waypoint waypoint = GetComponent<Waypoint>();
            if (waypoint)
            {
                waypoint.Thing = null;
            }
        }
    }
}
