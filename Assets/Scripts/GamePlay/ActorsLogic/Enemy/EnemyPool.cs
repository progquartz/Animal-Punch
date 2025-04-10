using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> enemyPool = new Dictionary<string, Queue<GameObject>>();

    /// <summary>
    /// 풀을 미리 초기화합니다.
    /// </summary>
    public void InitializePool(string key, int initialSize)
    {
        if (!enemyPool.ContainsKey(key))
        {
            Logger.Log($"{key}의 이름으로 새로운 오브젝트 풀링 시도");
            Queue<GameObject> newPool = new Queue<GameObject>();
            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = Instantiate(DataManager.Instance.EnemyDataStorage.GetEnemyOnKey(key).spawnPrefab);
                obj.SetActive(false);
                newPool.Enqueue(obj);
            }
            Logger.Log($"{key}의 이름으로 새로운 오브젝트 풀링 성공");
            enemyPool.Add(key, newPool);
        }
    }

    /// <summary>
    /// 요청된 키에 대해 풀에서 오브젝트를 반환합니다. 없으면 새로 생성.
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
                // 풀에 준비된 객체가 없으면 새로 생성
                // enemydatastorage에서 올바른 데이터 받아오기...
                return Instantiate(DataManager.Instance.EnemyDataStorage.GetEnemyOnKey(key).spawnPrefab);
            }
        }
        else
        {
            // 키가 없으면 초기화 후 반환
            InitializePool(key, 1);
            return GetFromPool(key);
        }
    }

    /// <summary>
    /// 사용이 끝난 오브젝트를 풀에 반환합니다.
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
