using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.agent;
using wizardscode.agent.movement;
using wizardscode.environment;
using wizardscode.production;
using wizardscode.utility;

namespace wizardscode.digitalpainting.agent
{
    public class AIAgentController : BaseAIAgentController
    {        
        private RobotMovementController pathfinding;

        override internal void Awake()
        {
            base.Awake();

            pathfinding = GetComponent<RobotMovementController>();
            if (pathfinding == null)
            {
                Debug.LogWarning("No RobotMovementController found on " + gameObject.name + ". One has been added automatically, but consider adding one manually so that it may be optimally configured.");
                pathfinding = gameObject.AddComponent<RobotMovementController>();
            }
        }

        override internal bool IsValidWaypoint(Vector3 position)
        {
            return pathfinding.Octree.IsTraversableCell(position);
        }

        override public Transform Target
        {
            get { return m_target; }
            set
            {
                base.Target = value;
                pathfinding.Target = value;
            }
        }
    }
}
