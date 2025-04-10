using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> enemyPool = new Dictionary<string, Queue<GameObject>>();

    /// <summary>
    /// Ǯ�� �̸� �ʱ�ȭ�մϴ�.
    /// </summary>
    public void InitializePool(string key, int initialSize)
    {
        if (!enemyPool.ContainsKey(key))
        {
            Logger.Log($"{key}�� �̸����� ���ο� ������Ʈ Ǯ�� �õ�");
            Queue<GameObject> newPool = new Queue<GameObject>();
            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = Instantiate(DataManager.Instance.EnemyDataStorage.GetEnemyOnKey(key).spawnPrefab);
                obj.SetActive(false);
                newPool.Enqueue(obj);
            }
            Logger.Log($"{key}�� �̸����� ���ο� ������Ʈ Ǯ�� ����");
            enemyPool.Add(key, newPool);
        }
    }

    /// <summary>
    /// ��û�� Ű�� ���� Ǯ���� ������Ʈ�� ��ȯ�մϴ�. ������ ���� ����.
    /// </summary>
    public GameObject GetFromPool(string key)
    {
        if (enemyPool.ContainsKey(key))
        {
            if (enemyPool[key].Count > 0)
            {
                GameObject obj = enemyPool[key].Dequeue();
                obj.SetActive(true);
                return obj;
            }
            else
            {
                // Ǯ�� �غ�� ��ü�� ������ ���� ����
                // enemydatastorage���� �ùٸ� ������ �޾ƿ���...
                return Instantiate(DataManager.Instance.EnemyDataStorage.GetEnemyOnKey(key).spawnPrefab);
            }
        }
        else
        {
            // Ű�� ������ �ʱ�ȭ �� ��ȯ
            InitializePool(key, 1);
            return GetFromPool(key);
        }
    }

    /// <summary>
    /// ����� ���� ������Ʈ�� Ǯ�� ��ȯ�մϴ�.
    /// </summary>
    public void ReturnToPool(string key, GameObject obj)
    {
        obj.SetActive(false);
        if (enemyPool.ContainsKey(key))
        {
            enemyPool[key].Enqueue(obj);
        }
        else
        {
            Queue<GameObject> newPool = new Queue<GameObject>();
            newPool.Enqueue(obj);
            enemyPool.Add(key, newPool);
        }
    }
}
