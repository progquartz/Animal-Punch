using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyPool : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> enemyPool = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, int> usingEnemyCount = new Dictionary<string, int>();
    


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
                GameObject obj = Instantiate(DataManager.Instance.EnemyDataStorage.GetEnemyBasePrefab(key));
                obj.SetActive(false);
                newPool.Enqueue(obj);
            }
            Logger.Log($"{key}의 이름으로 새로운 오브젝트 풀링 성공");
            enemyPool.Add(key, newPool);
            if(!usingEnemyCount.ContainsKey(key))
            {
                usingEnemyCount.Add(key, 0);
            }
        }
    }

    /// <summary>
    /// 요청된 키에 대해 풀에서 오브젝트를 반환합니다. 없으면 새로 생성.
    /// </summary>
    public GameObject GetFromPool(string key)
    {
//        Debug.Log("호출");
        if (enemyPool.ContainsKey(key))
        {
            usingEnemyCount[key]++;
            
            if (enemyPool[key].Count > 0)
            {
                Debug.Log($"{key}이름의 적을 Pooling하여 {usingEnemyCount[key]}개 있습니다.");
                GameObject obj = enemyPool[key].Dequeue();
                obj.GetComponent<Enemy>().IsInPool = false;
                obj.SetActive(true);
                return obj;
            }
            else
            {
                Debug.Log($"{key}이름의 적이 Pool에 없어 소환해 현재 {usingEnemyCount[key]}개 있습니다.");
                GameObject obj = Instantiate(DataManager.Instance.EnemyDataStorage.GetEnemyBasePrefab(key));
                obj.GetComponent<Enemy>().IsInPool = false;
                return obj;
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
        Enemy enemy = obj.GetComponent<Enemy>();    
        if(enemy == null || enemy.IsInPool)
        {
            return;
        }

        obj.SetActive(false);


        if (enemyPool.ContainsKey(key))
        {
            Logger.Log($"EnemyPool에 [{key}]를 key값으로 가지는 {obj.name}오브젝트가 풀로 돌아왔습니다.");
            enemy.IsInPool = true;
            usingEnemyCount[key]--;
            enemyPool[key].Enqueue(obj);
        }
        else
        {
            // 만약 처음부터 있는 엔티티가 있을 경우, 이는 조건에 포함되지 않으므로 에러를 띄워놓아야 하긴 함.
            // 우선 예외처리는 해둠.
            Logger.LogError($"현재 EnemyPool에 Initialize되어 pool에서 생성되지 않은 [{key}]를 key값으로 가지는 {obj.name}오브젝트가 리턴을 시도합니다.");
            Queue<GameObject> newPool = new Queue<GameObject>();
            newPool.Enqueue(obj);
            enemyPool.Add(key, newPool);
            usingEnemyCount.Add(key, 0);
        }
    }

    public int GetEnemyCount(string key)
    {
        if(usingEnemyCount.ContainsKey(key))
        {
            return usingEnemyCount[key];
        }
        usingEnemyCount.Add(key, 0);
        return usingEnemyCount[key];
    }

    public int GetAllEnemyCount()
    {
        return usingEnemyCount.Values.Sum();
    }


}
