using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.agent
{
    public class CameraDroneController : BaseAgentController
    {
        [Header("Camera Drone Controls")]
        [Tooltip("Allow manual control to override automated control?")]
        public bool allowManual = true;

        internal override void Update()
        {
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            if (allowManual)
            {
                base.Update();
            }

            if (position == transform.position && rotation == transform.rotation)
            {
                FlyByWire();
            }
        }

        /// <summary>
        /// Moves the drone according to the fly by wire program being carried.
        /// </summary>
        private void FlyByWire()
        {
            transform.position += transform.forward * normalMovementSpeed * Time.deltaTime;
        }
    }
}
