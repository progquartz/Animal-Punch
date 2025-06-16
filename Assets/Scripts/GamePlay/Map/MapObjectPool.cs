using System.Collections.Generic;
using UnityEngine;

public class MapObjectPool : SingletonBehaviour<MapObjectPool>
{
    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();


    protected override void Init()
    {
        IsDestroyOnLoad = true;
        base.Init();
    }

    public void InitializePool(string key, GameObject prefab, int initialSize)
    {
        if (!poolDictionary.ContainsKey(key))
        {
            Queue<GameObject> newPool = new Queue<GameObject>();
            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                newPool.Enqueue(obj);
            }
            poolDictionary.Add(key, newPool);
        }
    }

    public GameObject GetFromPool(string key, GameObject prefab, Transform parent)
    {
        GameObject obj;
        if (poolDictionary.ContainsKey(key) && poolDictionary[key].Count > 0)
        {
            obj = poolDictionary[key].Dequeue();
        }
        else
        {
            obj = Instantiate(prefab);
        }

        obj.SetActive(false);
        obj.transform.SetParent(parent, false);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.Sleep();  
        }

        obj.SetActive(true);  
        return obj;
    }


    public void ReturnToPool(string key, GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform); 

        if (!poolDictionary.ContainsKey(key))
        {
            poolDictionary[key] = new Queue<GameObject>();
        }

        poolDictionary[key].Enqueue(obj);
    }
}
