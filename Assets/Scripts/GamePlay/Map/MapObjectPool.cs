using System.Collections.Generic;
using UnityEngine;

public class MapObjectPool : MonoBehaviour
{
    // 키(예: "Rock")별로 풀에 저장된 GameObject 큐
    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    /// <summary>
    /// 풀을 미리 초기화합니다.
    /// </summary>
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
            Logger.Log($"{key}의 이름으로 새로운 오브젝트 풀링");
            poolDictionary.Add(key, newPool);
        }
    }

    /// <summary>
    /// 요청된 키에 대해 풀에서 오브젝트를 반환합니다. 없으면 새로 생성.
    /// </summary>
    public GameObject GetFromPool(string key, GameObject prefab)
    {
        if (poolDictionary.ContainsKey(key))
        {
            if (poolDictionary[key].Count > 0)
            {
                GameObject obj = poolDictionary[key].Dequeue();
                obj.SetActive(true);
                return obj;
            }
            else
            {
                // 풀에 준비된 객체가 없으면 새로 생성
                return Instantiate(prefab);
            }
        }
        else
        {
            // 키가 없으면 초기화 후 반환
            InitializePool(key, prefab, 1);
            return GetFromPool(key, prefab);
        }
    }

    /// <summary>
    /// 사용이 끝난 오브젝트를 풀에 반환합니다.
    /// </summary>
    public void ReturnToPool(string key, GameObject obj)
    {
        obj.SetActive(false);
        if (poolDictionary.ContainsKey(key))
        {
            poolDictionary[key].Enqueue(obj);
        }
        else
        {
            Queue<GameObject> newPool = new Queue<GameObject>();
            newPool.Enqueue(obj);
            poolDictionary.Add(key, newPool);
        }
    }
}
