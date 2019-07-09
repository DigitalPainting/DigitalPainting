using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsCode.utility
{
    /// <summary>
    /// A general object pool.
    /// To get an object from the pool simple call GetFromPool.
    /// To return an object to the pool call .SetActive(false) on it.
    /// </summary>
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool Instance;
        [Tooltip("A prefab that represents this object. If empty an empty game object will be created.")]
        public GameObject pooledObjectPrefab;
        [Tooltip("The number of objects to have available in the pool at startup.")]
        public int pooledAmountAtStartup = 20;
        [Tooltip("If an object is requested from the pool, but there are none available will a new one be added to the pool?")]
        public bool willGrow = true;

        List<GameObject> pooledObjects;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            pooledObjects = new List<GameObject>();
            for (int i = 0; i < pooledAmountAtStartup; i++)
            {
                CreateObjectInPool();
            }
        }

        private GameObject CreateObjectInPool()
        {
            GameObject obj;
            if (pooledObjectPrefab == null)
            {
                obj = new GameObject(name + " Object " + pooledObjects.Count);
            }
            else
            {
                obj = (GameObject)Instantiate(pooledObjectPrefab);
                obj.name = name + " Object " + pooledObjects.Count;
            }
            obj.SetActive(false);

            pooledObjects.Add(obj);

            return obj;
        }

        public GameObject GetFromPool()
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (!pooledObjects[i].activeInHierarchy)
                {
                    pooledObjects[i].SetActive(true);
                    return pooledObjects[i];
                }
            }

            // if we got here there are currently no inactive objects
            if (willGrow)
            {
                GameObject go = CreateObjectInPool();
                go.SetActive(true);
                return go;
            }
            else
            {
                return null;
            }

        }
    }
}
