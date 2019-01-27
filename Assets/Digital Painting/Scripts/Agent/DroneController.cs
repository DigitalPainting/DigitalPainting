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
        private RobotMovementController pathfinding;
        private Transform target;

        override internal void Awake()
        {
            base.Awake();
            pathfinding = GetComponent<RobotMovementController>();
            target = new GameObject("Wander Target for " + gameObject.name).transform;
        }

        internal override void Update()
        {
            if (isFlyByWire)
            {
                if (thingOfInterest != null)
                {
                    if (Vector3.Distance(transform.position, thingOfInterest.transform.position) > thingOfInterest.distanceToTriggerViewingCamera)
                    {
                        pathfinding.Target = thingOfInterest.gameObject;
                    } else
                    {
                        ViewPOI();
                    }
                } else
                {
                    UpdatePointOfInterest();
                    if (thingOfInterest == null)
                    {
                        timeToNextWanderPathChange -= Time.deltaTime;
                        if (timeToNextWanderPathChange < 0)
                        {
                            Vector3 position = transform.position + (Random.insideUnitSphere * 15);

                            // calculate the new height 
                            float newY = Terrain.activeTerrain.SampleHeight(position) + heightOffset;
                            float oldY = position.y;
                            float diffY = newY - oldY;
                            position.y += diffY * Time.deltaTime;

                            target.position = position;
                            pathfinding.Target = target.gameObject;
                            timeToNextWanderPathChange = Random.Range(minTimeBetweenRandomPathChanges, maxTimeBetweenRandomPathChanges);
                        }
                    }
                }
            }
        }
    }
}
