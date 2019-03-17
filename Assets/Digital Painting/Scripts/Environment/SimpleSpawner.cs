using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.environment {
    public class SimpleSpawner : MonoBehaviour
    {
        public SpawnableObject[] objects;
               
        private void Start()
        {
            for (int i = 0; i < objects.Length; i++) {
                Vector3 realCenter = objects[i].center;
                realCenter.y = Terrain.activeTerrain.SampleHeight(objects[i].center);
                for (int c = 0; c < objects[i].number; c++)
                {
                    float size = Random.Range(objects[i].minSize, 1);
                    Vector3 pos = (Random.insideUnitSphere * objects[i].radius) + realCenter;
                    if (objects[i].isGrounded)
                    {
                        pos.y = Terrain.activeTerrain.SampleHeight(pos) + (objects[i].yOffset * size);
                    }
                    GameObject obj = Instantiate(objects[i].prefab, pos, Quaternion.identity);
                    obj.transform.localScale = new Vector3(size, size, size);
                }
            }
        }
    }
}
