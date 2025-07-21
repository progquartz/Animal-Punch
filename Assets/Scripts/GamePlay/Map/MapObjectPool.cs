using System.Collections.Generic;
using System.Threading.Tasks;
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

    public async Task PreloadAllMapObjects(int defaultPoolSize = 10)
    {
        var mapDataStorage = DataManager.Instance.MapDataStorage;

        // ±×·ì ÇÁ¸®ÆÕ Ç®¸µ
        foreach (var groupPrefab in mapDataStorage.groupPrefabs)
        {
            InitializePool(groupPrefab.key, groupPrefab.gameObject, defaultPoolSize);
            await Task.Yield();
        }

        // ÇÁ¸®ÆÕ ¸ðµ¨¸µ Ç®¸µ
        foreach (var modelPrefab in mapDataStorage.ModelPrefabs)
        {
            InitializePool(modelPrefab.name, modelPrefab, defaultPoolSize);
            await Task.Yield();
        }

        Debug.Log("MapObjectPool: Preload completed.");
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

    public void ClearAllPools()
    {
        foreach (var queue in poolDictionary.Values)
        {
            while (queue.Count > 0)
            {
                GameObject obj = queue.Dequeue();
                if (obj != null)
                    Destroy(obj);
            }
        }
        poolDictionary.Clear();
    }

}
