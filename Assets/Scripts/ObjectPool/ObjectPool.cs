using System.Collections.Generic;
using UnityEngine;

// Set up the basic functionality for the object pool class
public class ObjectPool
{
    private PoolableObject Prefab;
    private int Size;
    private List<PoolableObject> AvailableObjectsPool;

    private ObjectPool(PoolableObject Prefab, int Size)
    {
        this.Prefab = Prefab;
        this.Size = Size;
        AvailableObjectsPool = new List<PoolableObject>(Size);
    }

    public static ObjectPool CreateInstance(PoolableObject Prefab, int Size)
    {
        ObjectPool pool = new ObjectPool(Prefab, Size);

        GameObject poolGameObject = new GameObject(Prefab + " Pool");
        pool.CreateObjects(poolGameObject);

        return pool;
    }

    private void CreateObjects(GameObject parent)
    {
        for (int i = 0; i < Size; i++)
        {
            PoolableObject poolableObject = GameObject.Instantiate(Prefab, Vector3.zero, Quaternion.identity, parent.transform);
            poolableObject.Parent = this;
            poolableObject.gameObject.SetActive(false); // PoolableObject handles re-adding the object to the AvailableObjects
        }
    }

    public void ReturnObjectToPool(PoolableObject Object)
    {
        AvailableObjectsPool.Add(Object);
    }

    public PoolableObject GetObject()
    {
        if (AvailableObjectsPool.Count > 0)
        {
            PoolableObject instance = AvailableObjectsPool[0];

            AvailableObjectsPool.RemoveAt(0);

            instance.gameObject.SetActive(true);

            return instance;
        }
        else
        {
            return null;
        }
    }
}