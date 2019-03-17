using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.environment
{
    public class SimpleSpawner : MonoBehaviour
    {
        public SpawnableObject[] objects;

        private void Awake()
        {
            for (int i = 0; i < objects.Length; i++)
            {
                GameObject parent = null;
                Vector3 realCenter = objects[i].center;
                realCenter.y = Terrain.activeTerrain.SampleHeight(objects[i].center);

                if (objects[i].createParent)
                {
                    parent = new GameObject("Spawned " + objects[i].prefab.name);
                    parent.transform.position = realCenter;

                    if (objects[i].isInterestingThing)
                    {
                        Thing thing = parent.AddComponent<Thing>();
                        BoxCollider collider = parent.GetComponent<BoxCollider>();
                        collider.size = Vector3.one;

                        GameObject view = new GameObject("Agent Viewing Position for " + parent.name);
                        Vector3 pos = new Vector3(realCenter.x + objects[i].radius, 0, realCenter.z + objects[i].radius);
                        pos.y = Terrain.activeTerrain.SampleHeight(pos) + objects[i].radius / 3;
                        view.transform.position = pos;
                        view.transform.LookAt(parent.transform.position);

                        view.transform.SetParent(parent.transform, true);
                        thing.AgentViewingTransform = view.transform;
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
                        Vector3 scale = obj.transform.lossyScale;
                        obj.transform.SetParent(parent.transform, true);
                        obj.transform.localScale = scale;
                    }
                }
            }
        }
    }
}
