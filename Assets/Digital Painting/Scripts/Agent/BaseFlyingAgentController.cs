using System;
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

        
        public override MovementStyle MovementType
        {
            get { return m_currentMovementStyle; }
            set {
                // FIXME: don't use strings to set animator parameters in below
                switch (value)
                {
                    case MovementStyle.IdleGrounded:
                        SlowToStop();
                        break;
                    case MovementStyle.Grounded:
                        if (!InTransition && currentHeight > 0)
                        {
                            InTransition = true;

                            if (IsMoving)
                            {
                                animator.SetBool(MovementController.movingLand, true);
                            }
                            else
                            {
                                animator.SetBool(MovementController.idleLand, true);
                            }
                        } else if (InTransition && currentHeight <= 0)
                        {
                            InTransition = false;
                        }
                        break;
                    case MovementStyle.TakingOff:
                        if (InTransition && currentHeight < MovementController.minimumFlyHeight)
                        {
                            if (MovementBrain.Speed > 0)
                            {
                                animator.SetBool(MovementController.movingTakeOff, true);
                            } else {
                                animator.SetBool(MovementController.idleTakeOff, true);
                            }
                        } else
                        {
                            InTransition = false;
                        }
                        break;
                    case MovementStyle.IdleFlying:
                        SlowToStop();
                        break;
                    case MovementStyle.Flying:
                        animator.SetBool(MovementController.endDive, true);
                        animator.SetBool(MovementController.endGlide, true);

                        if (!InTransition && m_currentMovementStyle == MovementStyle.Grounded)
                        {
                            InTransition = true;
                            if (IsMoving)
                            {
                                animator.SetBool(MovementController.movingTakeOff, true);
                            }
                            else
                            {
                                animator.SetBool(MovementController.idleTakeOff, true);
                            }
                        }

                        if (InTransition && currentHeight >= MovementController.minimumFlyHeight)
                        {
                            InTransition = false;
                        }

                        break;
                    case MovementStyle.Diving:
                        if (m_currentMovementStyle != MovementStyle.Diving)
                        {
                            animator.SetBool(MovementController.endDive, false);
                            animator.SetBool(MovementController.startDive, true);
                        }
                        break;
                    case MovementStyle.Gliding:
                        if (m_currentMovementStyle != MovementStyle.Gliding && m_currentMovementStyle != MovementStyle.Flying)
                        {
                            MovementType = MovementStyle.Flying;
                            return;
                        }
                        else if (m_currentMovementStyle != MovementStyle.Gliding)
                        {
                            animator.SetBool(MovementController.endGlide, false);
                            animator.SetBool(MovementController.glide, true);
                        }
                        break;
                }
                m_currentMovementStyle = value;
            }
        }

        /// <summary>
        /// Tests to see if the agent is currently flying.
        /// </summary>
        internal bool IsFlying {
            get { return MovementType != MovementStyle.Grounded; }
            set { MovementType = MovementStyle.Flying; }
        }

        /// <summary>
        /// Test to see if the agent is transitioning between flying and grounded. When this is
        /// true it means that the IsFLying value has just toggled, or will soon toggle.
        /// </summary>
        /// <returns>True if in a transitioning state.</returns>
        public bool InTransition { get; internal set; }

        /// <summary>
        /// Test to see if the agent is moving.
        /// </summary>
        /// <returns>True if in a transitioning state.</returns>
        public bool IsMoving {
            get { return animator.GetFloat("locomotion") != 0.5; }
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
                FlyUp();
            }

            // Set the turn angle
            float turnY = transform.eulerAngles.y + (currentRotation * MovementController.maxRotationSpeed * Math.Abs(MovementBrain.Speed / MovementController.maxSpeed) * Time.deltaTime);
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
        internal void ManageHeight()
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

                    if (!InTransition && currentHeight <= MovementController.minimumFlyHeight + 0.05f)
                    {
                        currentHeight = MovementController.minimumFlyHeight;
                        MovementType = MovementStyle.Grounded;
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
            MovementType = MovementStyle.Flying;
            if (MovementController.useRootMotion)
            {
                float angleChange = MovementController.maxRotationSpeed * Math.Abs(MovementBrain.Speed / MovementController.maxSpeed) * Time.deltaTime;
                currentFlyingAngle = Mathf.Clamp(currentFlyingAngle + angleChange, MovementController.maximumPitch, 0);
                SetFlyingRotation();
            }
            else
            {
                Debug.LogError("Currently BaseFlyingAgentController only supports agents with Root Motion animations.");
            }
        }

        internal void SetFlyingRotation()
        {
            Vector3 newRotation = new Vector3(currentFlyingAngle, transform.eulerAngles.y, transform.eulerAngles.z);
            transform.eulerAngles = newRotation;
        }

        virtual public void FlyUp()
        {
            MovementType = MovementStyle.Flying;
            if (!InTransition && MovementController.useRootMotion)
            {
                float angleChange = MovementController.maxRotationSpeed * Math.Abs(MovementBrain.Speed / MovementController.maxSpeed) * Time.deltaTime;
                currentFlyingAngle = Mathf.Clamp(currentFlyingAngle - angleChange , -MovementController.maximumPitch, 0);
                SetFlyingRotation();
            }
            else if (!InTransition)
            {
                Debug.LogError("Currently BaseFlyingAgentController only supports agents with Root Motion animations.");
            }
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
            Vector3 newRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -currentRotation * MovementController.maxRotationSpeed);
            if (IsFlying)
            {
                transform.eulerAngles = newRotation;
            }
        }
    }
}
