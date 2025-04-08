using System.Collections.Generic;
using UnityEngine;

public class MapObjectPool : MonoBehaviour
{
    // Ű(��: "Rock")���� Ǯ�� ����� GameObject ť
    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    /// <summary>
    /// Ǯ�� �̸� �ʱ�ȭ�մϴ�.
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
            Logger.Log($"{key}�� �̸����� ���ο� ������Ʈ Ǯ��");
            poolDictionary.Add(key, newPool);
        }
    }

    /// <summary>
    /// ��û�� Ű�� ���� Ǯ���� ������Ʈ�� ��ȯ�մϴ�. ������ ���� ����.
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
                // Ǯ�� �غ�� ��ü�� ������ ���� ����
                return Instantiate(prefab);
            }
        }
        else
        {
            // Ű�� ������ �ʱ�ȭ �� ��ȯ
            InitializePool(key, prefab, 1);
            return GetFromPool(key, prefab);
        }
    }

    /// <summary>
    /// ����� ���� ������Ʈ�� Ǯ�� ��ȯ�մϴ�.
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
