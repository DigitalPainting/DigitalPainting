using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wizardscode.digitalpainting.agent;

namespace wizardscode.agent
{
    public class BaseFlyingAgentController : BaseAgentController
    {
        internal float currentRotation;
        internal float currentHeight = 0;
        internal float currentFlyingAngle;

        /// <summary>
        /// Tests to see if the agent is currently flying.
        /// </summary>
        internal bool IsFlying { get; private set;  }
        
        /// <summary>
        /// Tests to see if the agent is currently landing.
        /// </summary>
        internal bool IsLanding { get; private set; }

        /// <summary>
        /// Test to see if the agent is transitioning between flying and grounded. When this is
        /// true it means that the IsFLying value has just toggled, or will soon toggle.
        /// </summary>
        /// <returns>True if in a transitioning state.</returns>
        public bool InTransition { get; private set; }

        /// <summary>
        /// Start a transition from flying to grounded or vice-versa. This will
        /// start the appropriate animation. The animation should call EndTransition
        /// when the transition to air or ground is complete (not necessarily when 
        /// the animation is complete).
        /// </summary>
        public void StartTransition()
        {
            InTransition = true;
            if (!IsFlying)
            {
                TakeOff();
            }
        }
        
        /// <summary>
        /// End a transition from flying to grounded or vice-versa. Typically
        /// this will be called from an animation event. The IsFlying status
        /// will be changed when this is called.
        /// </summary>
        public void EndTransition()
        {
            InTransition = false;
            if (IsFlying)
            {
                // Landed
                IsFlying = false;
                IsLanding = false;
                currentHeight = 0;
                AdjustHeightToTerrain();
            }
            else
            {
                // Taken Off
                IsFlying = true;
            }
        }
        
        public override void Move()
        {
            base.Move();
            float previousTerrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);

            if (Input.GetKey(KeyCode.Q))
            {
                FlyUp();
            }

            if (Input.GetKey(KeyCode.E))
            {
                FlyDown();
            }

            if (!InTransition && Input.GetKey(KeyCode.F))
            {
                StartTransition();
            }

            // Set the turn angle
            float turnY = transform.eulerAngles.y + (currentRotation * MovementController.rotationSpeed * Time.deltaTime);
            Vector3 newRotation = new Vector3(transform.eulerAngles.x, turnY, transform.eulerAngles.z);
            transform.eulerAngles = newRotation;
            
            if (!InTransition)
            {
                AdjustHeightToTerrain();
            }
            ManageHeight();
        }

        /// <summary>
        /// Sets the current height to the correct value above the terrain. 
        /// </summary>
        private void ManageHeight()
        {
            float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
            currentHeight = transform.position.y - terrainHeight;

            // if landing get to a height where the animation will work then fire the animations
            if (InTransition)
            {
                if (IsFlying)
                {
                    if (currentHeight > MovementController.minimumFlyHeight + 0.05f)
                    {
                        currentHeight -= MovementController.climbSpeed * Time.deltaTime;
                        currentFlyingAngle = 0;
                        SetFlyingRotation();
                    }

                    if (!IsLanding && currentHeight <= MovementController.minimumFlyHeight + 0.05f)
                    {
                        currentHeight = MovementController.minimumFlyHeight;
                        IsLanding = true;
                        Land();
                    }
                    else
                    {
                        Vector3 newPos = new Vector3(transform.position.x, terrainHeight + currentHeight, transform.position.z);
                        transform.position = newPos;
                    }
                }
            }
            else if (IsFlying)
            {
                if (currentHeight < MovementController.minimumFlyHeight)
                {
                    currentHeight += MovementController.climbSpeed * Time.deltaTime;
                }
            }
        }

        /// <summary>
        /// Modify the models height so that it remains a constant height above the terrain.
        /// </summary>
        /// <param name="terrainHeight"></param>
        private void AdjustHeightToTerrain()
        {
            float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);

            Vector3 newPos = transform.position;
            newPos.y = currentHeight + terrainHeight + MovementController.heightOffset;
            transform.position = newPos;
        }

        virtual public void FlyDown()
        {
            if (MovementController.useRootMotion)
            {
                float angleChange = MovementController.rotationSpeed * Time.deltaTime;
                currentFlyingAngle = Mathf.Clamp(currentFlyingAngle + angleChange, MovementController.maximumFlyingAngle, 0);
                SetFlyingRotation();
            }
            else
            {
                Debug.LogError("Currently BaseFlyingAgentController only supports agents with Root Motion animations.");
            }
        }

        private void SetFlyingRotation()
        {
            Vector3 newRotation = new Vector3(currentFlyingAngle, transform.eulerAngles.y, transform.eulerAngles.z);
            transform.eulerAngles = newRotation;
        }


        /// <summary>
        /// Perform the appropriate Landing animation.
        /// </summary>
        virtual public void Land()
        {
            
        }

        virtual public void FlyUp()
        {
            if (currentHeight <= 0)
            {
                StartTransition();
            }
            else if (MovementController.useRootMotion)
            {
                float angleChange = MovementController.rotationSpeed * Time.deltaTime;
                currentFlyingAngle = Mathf.Clamp(currentFlyingAngle - angleChange , -MovementController.maximumFlyingAngle, 0);
                SetFlyingRotation();
            }
            else
            {
                Debug.LogError("Currently BaseFlyingAgentController only supports agents with Root Motion animations.");
            }
        }

        /// <summary>
        /// Perform the appropriate TakeOff animation.
        /// </summary>
        virtual public void TakeOff()
        {
        }

        internal override void MoveVerticalAxis(float speedMultiplier)
        {
            float locomotion = 0.5f;
            float axis = Input.GetAxis("Vertical");
            if (axis > 0)
            {
                locomotion = Mathf.Clamp(MovementController.normalMovementSpeed * speedMultiplier * axis, 0.51f, MovementController.maxSpeed);
            }
            else if (axis < 0)
            {
                locomotion = Mathf.Clamp(MovementController.normalMovementSpeed * speedMultiplier * (1 + axis), 0.0f, 0.49f);
            }
            animator.SetFloat("locomotion", locomotion);
        }

        internal override void MoveHorizontalAxis(float speedMultiplier)
        {
            currentRotation = Input.GetAxis("Horizontal");
            Vector3 newRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -currentRotation * MovementController.rotationSpeed);
            if (IsFlying)
            {
                transform.eulerAngles = newRotation;
            }
        }
    }
}
