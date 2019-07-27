using Cinemachine;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WizardsCode.Environment
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
                realCenter.y = UnityEngine.Terrain.activeTerrain.SampleHeight(objects[i].center);

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
                        pos.y = UnityEngine.Terrain.activeTerrain.SampleHeight(pos) + objects[i].radius / 3;
                        view.transform.position = pos;
                        view.transform.LookAt(parent.transform.position);
                        view.transform.SetParent(parent.transform, true);
                        thing.AgentViewingTransform = view.transform;

                        GameObject camera = new GameObject();
                        camera.name = "Virtual Camera for " + this.name;
                        CinemachineVirtualCamera virtualCamera = camera.AddComponent<CinemachineVirtualCamera>();
                        virtualCamera.m_StandbyUpdate = CinemachineVirtualCameraBase.StandbyUpdateMode.Never;
                        virtualCamera.LookAt = parent.transform;
                        virtualCamera.Follow = parent.transform;

                        CinemachineFramingTransposer transposer = virtualCamera.AddCinemachineComponent<CinemachineFramingTransposer>();
                        transposer.m_CameraDistance = objects[i].radius * 2;

                        virtualCamera.AddCinemachineComponent<CinemachineComposer>();
                        virtualCamera.transform.SetParent(parent.transform, true);
                        thing._virtualCamera = virtualCamera;
                        virtualCamera.enabled = false;
                    }
                }

                for (int c = 0; c < objects[i].number; c++)
                {
                    float size = Random.Range(objects[i].minSize, 1);
                    Vector3 pos = (Random.insideUnitSphere * objects[i].radius) + realCenter;
                    if (objects[i].isGrounded)
                    {
                        pos.y = UnityEngine.Terrain.activeTerrain.SampleHeight(pos) + (objects[i].yOffset * size);
                    }
                    Quaternion angle = Quaternion.Euler(0, Random.Range(0, objects[i].randomAngle), 0);
                    GameObject obj = Instantiate(objects[i].prefab);
                    obj.transform.localScale = new Vector3(size, size, size);
                    obj.transform.position = pos;
                    obj.transform.rotation = angle;

                    if (parent != null)
                    {
                        Vector3 scale = obj.transform.lossyScale;
                        obj.transform.SetParent(parent.transform, true);
                        obj.transform.localScale = scale;
                    }

                    CustomizeObject(obj, objects[i]);
                }
            }
        }


        /// <summary>
        /// Do per object customization during initial instantiation.
        /// This method is intended to be overridden in classes that extend the SimpleSpawner to provide specific object customizations.
        /// </summary>
        /// <param name="go">The GameObject to customize.</param>
        /// <param name="spawnerDefinition">The spawner definition that created this object.</param>
        internal virtual void CustomizeObject(GameObject go, SpawnableObject spawnerDefinition)
        {

        }
    }
}
