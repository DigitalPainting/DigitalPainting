using UnityEngine;

namespace wizardscode.agent.movement
{
    /// <summary>
    /// The movement brain controls movement of an agent. Through the configuration of various settings
    /// you can create a variety of movement types from fully manual to fully automated.
    /// </summary>
    [CreateAssetMenu(fileName = "Movement Controller", menuName = "Wizards Code/Agent/Manual Movement Controller")]
    public class ManualMovementControllerSO : MovementControllerSO
    {
        /// <summary>
        /// Typically the Move method is called from the Update method of the agent controller.
        /// It is responsible for making a decision about the agents next move and acting upon
        /// that decision.
        /// <paramref name="transform">The transform of the agent to be moved.</paramref>
        /// </summary>
        override internal void Move(Transform transform)
        {
            // Move with the keyboard controls 
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                transform.position += transform.forward * (normalMovementSpeed * fastMovementFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += transform.right * (normalMovementSpeed * fastMovementFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                transform.position += transform.forward * (normalMovementSpeed * slowMovementFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += transform.right * (normalMovementSpeed * slowMovementFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
            }
            else
            {
                transform.position += transform.forward * normalMovementSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += transform.right * normalMovementSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.Q))
            {
                heightOffset += climbSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.E))
            {
                heightOffset -= climbSpeed * Time.deltaTime;
            }
        }
    }
}