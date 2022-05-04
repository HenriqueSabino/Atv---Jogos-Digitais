using System.Collections.Generic;
using UnityEngine;

class Pool : IPooledObjectListener
{
    public int MaxSize { get; private set; }
    public GameObject PooledObject { get; set; }
    public Transform Parent;

    public List<PooledObject> activeObjects { get; private set; }
    private List<PooledObject> inactiveObjects;

    public Pool(Transform parent, GameObject pooledObject, int maxSize)
    {
        Parent = parent;
        PooledObject = pooledObject;
        MaxSize = maxSize;

        activeObjects = new List<PooledObject>();
        inactiveObjects = new List<PooledObject>();

        for (int i = 0; i < MaxSize; i++)
        {
            PooledObject obj = GameObject.Instantiate(PooledObject, Vector3.zero, Quaternion.identity, Parent.transform).GetComponent<PooledObject>();
            obj.Listeners.Add(this);
            activeObjects.Add(obj);
            obj.gameObject.SetActive(false);
        }
    }

    public GameObject Spawn(Vector3 position, Quaternion rotation)
    {
        PooledObject spawnedObject = null;

        // Use inactive objects instead of instantiating
        if (inactiveObjects.Count > 0)
        {
            // Removing from inactive pool
            spawnedObject = inactiveObjects[0];
            inactiveObjects.Remove(spawnedObject);

            // Resetting object
            spawnedObject.transform.position = position;
            spawnedObject.transform.rotation = rotation;
            spawnedObject.gameObject.SetActive(true);

            // Moving active objects
            activeObjects.Add(spawnedObject);
        }
        else if (activeObjects.Count < MaxSize)
        {
            spawnedObject = GameObject.Instantiate(PooledObject, position, rotation, Parent).GetComponent<PooledObject>();
            spawnedObject.Listeners.Add(this);
            activeObjects.Add(spawnedObject);
        }

        return spawnedObject?.gameObject;
    }

    public void ResetPool()
    {
        for (int i = activeObjects.Count - 1; i >= 0; i--)
        {
            activeObjects[i].gameObject.SetActive(false);
        }
    }

    public void IncreasePoolSize(int amount)
    {
        if (amount > 0)
        {
            MaxSize += amount;

            for (int i = 0; i < amount; i++)
            {
                PooledObject obj = GameObject.Instantiate(PooledObject, Vector3.zero, Quaternion.identity, Parent.transform).GetComponent<PooledObject>();
                obj.Listeners.Add(this);
                activeObjects.Add(obj);
                obj.gameObject.SetActive(false);
            }
        }
    }

    public void OnObjectDisabled(PooledObject obj)
    {
        activeObjects.Remove(obj);
        inactiveObjects.Add(obj);
    }
}