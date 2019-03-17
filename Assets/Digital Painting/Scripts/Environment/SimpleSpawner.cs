using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.environment {
    public class SimpleSpawner : MonoBehaviour
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
               
        private void Start()
        {
            Vector3 realCenter = center;
            realCenter.y = Terrain.activeTerrain.SampleHeight(center);
            for (int i = 0; i < number; i++)
            {
                float size = Random.Range(minSize, 1);
                Vector3 pos = (Random.insideUnitSphere * radius) + realCenter;
                if (isGrounded)
                {
                    pos.y = Terrain.activeTerrain.SampleHeight(pos) + (yOffset * size);
                }
                GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
                obj.transform.localScale = new Vector3(size, size, size);
            }
        }
    }
}
