using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.Utility;

namespace WizardsCode.Environment
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
