using UnityEngine;

namespace wizardscode.agent {
    public class BaseMovementController : MonoBehaviour
    {
        [Header("Core Movement")]
        protected Transform target;
        [SerializeField] protected float acceleration = 1;
        [Tooltip("The minimum distance an agent must get from an object before it is considered to have reached it.")]
        public float minReachDistance = 2f;

        protected Vector3 currentDestination;
        protected Vector3 lastDestination;

        public Transform Target
        {
            get { return target; }
            set { target = value; }
        }

        private void OnDrawGizmosSelected()
        {
            if (GetComponent<Rigidbody>() != null)
            {
                Gizmos.color = Color.blue;
                Vector3 predictedPosition = GetComponent<Rigidbody>().position + GetComponent<Rigidbody>().velocity * Time.deltaTime;
                if (GetComponent<Collider>().GetType() == typeof(SphereCollider))
                {
                    Gizmos.DrawWireSphere(predictedPosition, ((SphereCollider)GetComponent<Collider>()).radius);
                } else if (GetComponent<Collider>().GetType() == typeof(CapsuleCollider))
                {
                    Gizmos.DrawWireSphere(predictedPosition, ((CapsuleCollider)GetComponent<Collider>()).radius);
                } else
                {
                    Gizmos.DrawWireCube(predictedPosition, GetComponent<Collider>().bounds.size);
                }
            }
        }
    }
}