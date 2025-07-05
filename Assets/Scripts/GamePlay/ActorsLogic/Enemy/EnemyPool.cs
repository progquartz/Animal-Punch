using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> enemyPool = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, int> usingEnemyCount = new Dictionary<string, int>();

    public void InitializePool(string key, int initialSize)
    {
        if (!enemyPool.ContainsKey(key))
        {
            Queue<GameObject> newPool = new Queue<GameObject>();
            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = Instantiate(DataManager.Instance.EnemyDataStorage.GetEnemyBasePrefab(key));
                obj.SetActive(false);
                newPool.Enqueue(obj);
            }
            enemyPool.Add(key, newPool);
            if (!usingEnemyCount.ContainsKey(key))
            {
                usingEnemyCount.Add(key, 0);
            }
        }
    }

    public GameObject GetFromPool(string key, Transform parent, Vector3 position, Quaternion rotation, bool isDummy = false)
    {
        GameObject obj;

        if (enemyPool.ContainsKey(key) && enemyPool[key].Count > 0)
        {
            obj = enemyPool[key].Dequeue();
        }
        else
        {
            obj = Instantiate(DataManager.Instance.EnemyDataStorage.GetEnemyBasePrefab(key));
        }

        obj.SetActive(false);  // 활성화 전 설정
        obj.transform.SetParent(parent, false);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.Sleep();
        }

        Enemy enemy = obj.GetComponent<Enemy>();
        enemy.IsInPool = false;
        obj.SetActive(true);

        usingEnemyCount[key]++;
        return obj;
    }

    public void ReturnToPool(string key, GameObject obj)
    {
        Enemy enemy = obj.GetComponent<Enemy>();
        if (enemy == null || enemy.IsInPool) return;

        obj.SetActive(false);
        obj.transform.SetParent(transform);

        enemy.IsInPool = true;
        usingEnemyCount[key]--;

        if (!enemyPool.ContainsKey(key))
            enemyPool[key] = new Queue<GameObject>();

        enemyPool[key].Enqueue(obj);
    }

    public int GetEnemyCount(string key) => usingEnemyCount.ContainsKey(key) ? usingEnemyCount[key] : 0;

    public int GetAllEnemyCount() => usingEnemyCount.Values.Sum();
}
