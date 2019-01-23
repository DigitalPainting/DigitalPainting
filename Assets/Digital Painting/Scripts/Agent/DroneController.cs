using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.digitalpainting.agent
{
    /// <summary>
    /// DroneController is a FlyByWire or manual controller for a drone.
    /// 
    /// </summary>
    public class DroneController : AIAgentController
    {
        internal override void Update()
        {
            base.Update();
            SetHeight();
        }

        /// <summary>
        /// Set the current height of the drone based on the terrain height.
        /// </summary>
        private void SetHeight()
        {
            // get the current position and height above the terrain
            Vector3 position = transform.position;
            
            // calculate the new height 
            float newY = Terrain.activeTerrain.SampleHeight(position) + heightOffset;
            float oldY = position.y;
            float diffY = newY - oldY;
            position.y += diffY * Time.deltaTime;

            transform.position = position;
        }
    }
}
