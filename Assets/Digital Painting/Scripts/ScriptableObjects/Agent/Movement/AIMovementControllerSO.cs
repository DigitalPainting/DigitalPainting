using UnityEngine;
using wizardscode.digitalpainting;
using wizardscode.utility;

namespace wizardscode.agent.movement
{
    /// <summary>
    /// The movement brain controls movement of an agent. Through the configuration of various settings
    /// you can create a variety of movement types from fully manual to fully automated.
    /// </summary>
    [CreateAssetMenu(fileName = "Flying AI Movement Controller", menuName = "Wizards Code/Agent/Flying AI Movement Controller")]
    public class AIMovementControllerSO : MovementControllerSO
    {
        [Header("Wander configuration")]
        [Tooltip("Minimum time between random variations in the path.")]
        [Range(0, 120)]
        public float minTimeBetweenRandomPathChanges = 5;
        [Tooltip("Maximum time between random variations in the path.")]
        [Range(0, 120)]
        public float maxTimeBetweenRandomPathChanges = 15;
        [Tooltip("Minimum angle to change path when randomly varying")]
        [Range(-180, 180)]
        public float minAngleOfRandomPathChange = -25;
        [Tooltip("Maximum angle to change path when randomly varying")]
        [Range(-180, 180)]
        public float maxAngleOfRandomPathChange = 25;
        [Tooltip("Minimum distance to set a new wander target.")]
        [Range(1, 100)]
        public float minDistanceOfRandomPathChange = 10;
        [Tooltip("Maximum distance to set a new wander target.")]
        [Range(1, 100)]
        public float maxDistanceOfRandomPathChange = 25;
        
    }
}