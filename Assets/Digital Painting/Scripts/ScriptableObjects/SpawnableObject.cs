using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.environment
{
    [CreateAssetMenu(fileName = "Spawnable Object", menuName = "Wizards Code/Environment/Spawnable Object")]
    public class SpawnableObject : ScriptableObject
    {
        [Tooltip("Prefab to spawn.")]
        public GameObject prefab;
        [Tooltip("Position of the center point of the spawn area.")]
        public Vector3 center = Vector3.zero;
        [Tooltip("Radius within which to spawn the objects.")]
        public float radius = 10f;
        [Tooltip("Is the item to be spawned at ground level?")]
        public bool isGrounded = true;
        [Tooltip("Number to be spawned.")]
        public int number = 10;
        [Tooltip("Additional Y offset. This can be used to do things like ensure the trunk of a tree completely penetrates the ground.")]
        public float yOffset = 0;
        [Tooltip("Minimum size. All objects spawned will be a size of (1, 1, 1) this value.")]
        public float minSize = 1;
    }
}
