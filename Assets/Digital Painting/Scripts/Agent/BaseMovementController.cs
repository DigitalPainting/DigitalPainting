using UnityEngine;
using wizardscode.ai;

namespace wizardscode.agent {
    public class BaseMovementController : MonoBehaviour
    {
        protected Vector3 lastDestination;

        protected BaseMovementBrain MovementBrain;

        private void Awake()
        {
            MovementBrain = GetComponent<BaseMovementBrain>();
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