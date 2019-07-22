using UnityEngine;
using System;

namespace WizardsCode.Utility {
    /// <summary>
    /// Finds the slope/grade/incline angle of ground underneath an agent.
    /// 
    /// To use call GroundChecker.Instance.CheckGround(position) and then read the results from
    /// groundSlopeAngle and groundSlopeDir
    /// 
    /// Based on a blog post published by The Hidden Signal: http://thehiddensignal.com/unity-angle-of-sloped-ground-under-player/
    /// </summary>
    public class GroundChecker
    {
        [Header("Results")]
        public float groundSlopeAngle = 0f;            // Angle of the slope in degrees
        public Vector3 groundSlopeDir = Vector3.zero;  // The calculated slope as a vector

        [Header("Settings")]
        public bool showDebug = false;                  // Show debug gizmos and lines
        public LayerMask castingMask;                  // Layer mask for casts. You'll want to ignore the player.
        public float startDistanceFromBottom = 0.2f;   // Should probably be higher than skin width
        public float sphereCastRadius = 0.25f;
        public float sphereCastDistance = 0.75f;       // How far spherecast moves down from origin point

        public float raycastLength = 0.75f;
        public Vector3 rayOriginOffset1 = new Vector3(-0.2f, 0f, 0.16f);
        public Vector3 rayOriginOffset2 = new Vector3(0.2f, 0f, -0.16f);

        public static GroundChecker m_instance;
        public static GroundChecker Instance
        {
            get {
                if (m_instance == null)
                {
                    m_instance = new GroundChecker();
                }
                return m_instance;
            }
        }

        /// <summary>
        /// Checks for ground underneath, to determine some info about it, including the slope angle.
        /// </summary>
        /// <param name="origin">Point to start checking downwards from</param>
        public void CheckGround(Vector3 origin)
        {
            // Out hit point from our cast(s)
            RaycastHit hit;

            // SPHERECAST
            // "Casts a sphere along a ray and returns detailed information on what was hit."
            if (Physics.SphereCast(origin, sphereCastRadius, Vector3.down, out hit, sphereCastDistance, castingMask))
            {
                // Angle of our slope (between these two vectors). 
                // A hit normal is at a 90 degree angle from the surface that is collided with (at the point of collision).
                // e.g. On a flat surface, both vectors are facing straight up, so the angle is 0.
                groundSlopeAngle = Vector3.Angle(hit.normal, Vector3.up);

                // Find the vector that represents our slope as well. 
                //  temp: basically, finds vector moving across hit surface 
                Vector3 temp = Vector3.Cross(hit.normal, Vector3.down);
                //  Now use this vector and the hit normal, to find the other vector moving up and down the hit surface
                groundSlopeDir = Vector3.Cross(temp, hit.normal);
            }

            // Now that's all fine and dandy, but on edges, corners, etc, we get angle values that we don't want.
            // To correct for this, let's do some raycasts. You could do more raycasts, and check for more
            // edge cases here. There are lots of situations that could pop up, so test and see what gives you trouble.
            RaycastHit slopeHit1;
            RaycastHit slopeHit2;

            // FIRST RAYCAST
            if (Physics.Raycast(origin + rayOriginOffset1, Vector3.down, out slopeHit1, raycastLength))
            {
                // Debug line to first hit point
                if (showDebug) { Debug.DrawLine(origin + rayOriginOffset1, slopeHit1.point, Color.red); }
                // Get angle of slope on hit normal
                float angleOne = Vector3.Angle(slopeHit1.normal, Vector3.up);

                // 2ND RAYCAST
                if (Physics.Raycast(origin + rayOriginOffset2, Vector3.down, out slopeHit2, raycastLength))
                {
                    // Debug line to second hit point
                    if (showDebug) { Debug.DrawLine(origin + rayOriginOffset2, slopeHit2.point, Color.red); }
                    // Get angle of slope of these two hit points.
                    float angleTwo = Vector3.Angle(slopeHit2.normal, Vector3.up);
                    // 3 collision points: Take the MEDIAN by sorting array and grabbing middle.
                    float[] tempArray = new float[] { groundSlopeAngle, angleOne, angleTwo };
                    Array.Sort(tempArray);
                    groundSlopeAngle = tempArray[1];
                }
                else
                {
                    // 2 collision points (sphere and first raycast): AVERAGE the two
                    float average = (groundSlopeAngle + angleOne) / 2;
                    groundSlopeAngle = average;
                }
            }
        }

        void DrawGizmos(Vector3 position)
        {
            if (showDebug)
            {
                // Visualize SphereCast with two spheres and a line
                Vector3 startPoint = new Vector3(position.x, position.y + startDistanceFromBottom, position.z);
                Vector3 endPoint = new Vector3(position.x, position.y + startDistanceFromBottom - sphereCastDistance, position.z);

                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(startPoint, sphereCastRadius);

                Gizmos.color = Color.gray;
                Gizmos.DrawWireSphere(endPoint, sphereCastRadius);

                Gizmos.DrawLine(startPoint, endPoint);
            }
        }
    }
}