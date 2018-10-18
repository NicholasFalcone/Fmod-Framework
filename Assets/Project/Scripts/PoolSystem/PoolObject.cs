using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public static PoolObject instance;

    public Pool[] pool;
    public List<PoolItem> items;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitPool();
    }

    public void InitPool()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            //Create new pool an assign the name
            items.Add(new PoolItem() { key = pool[i].poolName });
            //Check the connectio with parent (if == null get in child of this gameObject)
            Transform curConnect = pool[i].connection;
            if (curConnect == null)
            {
                curConnect = transform;
            }

            for (int j = 0; j < pool[i].quantity; j++)
            {
                items[i].pool.Add(Instantiate(pool[i].prf_Object,new Vector2(152,150),Quaternion.identity, curConnect));
                items[i].pool[j].SetActive(false);
            }
        }

    }
    
    /// <summary>
    /// Take a gameObject on a pool with the same key
    /// </summary>
    /// <param name="poolName"></param>
    /// <param name="active">return gameobject active</param>
    /// <returns></returns>
    public static GameObject GetObject(string poolName, bool active = true)
    {
        PoolItem curPoolItem = FindPool(poolName);

        for (int i = 0; i < curPoolItem.pool.Count; i++)
        {
            if (!curPoolItem.pool[i].activeSelf)
            {
                if(active)
                    curPoolItem.pool[i].SetActive(true);
                return curPoolItem.pool[i];

            }
        }
        return null;
        throw new System.Exception("Pool Out of range");
    }

    public static GameObject GetObject(string poolName,Vector3 position, Vector3 rotation, bool active = true)
    {
        PoolItem curPoolItem = FindPool(poolName);

        for (int i = 0; i < curPoolItem.pool.Count; i++)
        {
            if (!curPoolItem.pool[i].activeSelf)
            {
                if (active)
                    curPoolItem.pool[i].SetActive(true);

                curPoolItem.pool[i].transform.position = position;
                curPoolItem.pool[i].transform.eulerAngles = rotation;
                return curPoolItem.pool[i];
            }
        }
        return null;
        throw new System.Exception("Pool Out of range");
    }

    public static PoolItem FindPool(string poolName)
    {
        for (int i = 0; i < instance.items.Count; i++)
        {
            if(instance.items[i].key == poolName)
            {
                return instance.items[i];
            }
        }
        throw new System.Exception("Pool Item dosent found");
    }

}
