using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 현재 Parenting 이슈 관련으로 문제가 있는 클래스입니다.
/// 새로 instancing을 해서 객체를 가져오자마자 parent를 설정해주지 않는다면 parenting issue로 gameobject의 position이 worldposition을 기준으로 zero로 위치가 강제됩니다.
/// 이를 해결하기위해, 비활성화 상태에서 객체를 가지고 온 뒤 parent로 둔 다음, 
/// 코루틴 등의 방법을 이용해 1 프레임 뒤에 localposition을 zero로 이동, 후에 활성화 하는 방법을 취하도록 한다.
/// </summary>
[Obsolete("현재 parenting 이슈로 사용할 수 없으니 수정 후 사용 필요.")]
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
