using UnityEngine;

namespace wizardscode.agent
{
    public class AIAgentController : BaseAgentController
    {
        [Header("AI Controller")]
        [Tooltip("Is the agent automated or manual movement?")]
        public bool isAutomatedMovement = true;
        [Tooltip("Current object of interest. The agent will move to and around the object until it is no longer interested, then it will make this parameter null. When null the agent will move according to other algorithms.")]
        public GameObject objectOfInterest;
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

        [Header("Overrides")]
        [Tooltip("Collider within which the drone must stay. If null an object called 'SafeArea' is used.")]

        public Collider safeAreaCollider;
        internal Quaternion targetRotation;
        private float timeToNextPathChange = 3;

        private void Awake()
        {
            if (safeAreaCollider == null)
            {
                safeAreaCollider = GameObject.Find("SafeArea").GetComponent<Collider>();
            }
        }

        internal override void Update()
        {

            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            if (!isAutomatedMovement)
            {
                base.Update();
            }
            else
            {

                if (!safeAreaCollider.bounds.Contains(transform.position))
                {
                    targetRotation = Quaternion.LookRotation(safeAreaCollider.bounds.center - transform.position, Vector3.up);
                }
                else
                {
                    SetupNextMovement();
                }
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.position += transform.forward * normalMovementSpeed * Time.deltaTime;
        }

        /// <summary>
        /// Sets up the state of the agent such that the next movement can be made by changing position and rotation of the agent
        /// </summary>
        internal void SetupNextMovement()
        {
            if (objectOfInterest != null)
            {
                targetRotation = Quaternion.LookRotation(objectOfInterest.transform.position - transform.position, Vector3.up);
            }
            else
            {
                // add some randomness to the flight 
                timeToNextPathChange -= Time.deltaTime;
                if (timeToNextPathChange <= 0)
                {
                    float rotation = Random.Range(minAngleOfRandomPathChange, maxAngleOfRandomPathChange);
                    Vector3 newRotation = targetRotation.eulerAngles;
                    newRotation.y += rotation;
                    targetRotation = Quaternion.Euler(newRotation);
                    timeToNextPathChange = Random.Range(minTimeBetweenRandomPathChanges, maxTimeBetweenRandomPathChanges);
                }
            }
        }
    }
}
