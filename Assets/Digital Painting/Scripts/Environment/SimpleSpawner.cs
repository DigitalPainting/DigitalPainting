using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.environment {
    public class SimpleSpawner : MonoBehaviour
    {
        public SpawnableObject[] objects;
               
        private void Awake()
        {
            for (int i = 0; i < objects.Length; i++) {
                GameObject parent = null;
                Vector3 realCenter = objects[i].center;
                realCenter.y = Terrain.activeTerrain.SampleHeight(objects[i].center);

                if (objects[i].createParent)
                {
                    parent = new GameObject("Spawned " + objects[i].prefab.name);
                    parent.transform.position = realCenter;
                    float size = objects[i].radius;
                    Bounds bounds = objects[i].prefab.GetComponent<Renderer>().bounds;
                    Vector3 scale = new Vector3(size, bounds.extents.y * 2.25f, size);
                    parent.transform.localScale = scale;

                    if (objects[i].isInterestingThing)
                    {
                        parent.AddComponent<Thing>();
                        BoxCollider collider = parent.GetComponent<BoxCollider>();
                        collider.size = Vector3.one;
                    }
                }

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

                    if (parent != null)
                    {
                        obj.transform.SetParent(parent.transform, true);
                    }
                }
            }
        }
    }
}
