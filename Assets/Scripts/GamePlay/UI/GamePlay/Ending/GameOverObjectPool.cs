using System.Collections.Generic;
using UnityEngine;

public class GameOverObjectPool : MonoBehaviour
{
    public GameObject dummyPrefab;
    private Queue<GameObject> dummyPool = new Queue<GameObject>();
    private Dictionary<string, Queue<GameObject>> visualPool = new Dictionary<string, Queue<GameObject>>();



    public GameObject GetDummy(float lifetime = 1.6f)
    {
        GameObject obj = (dummyPool.Count > 0) ? dummyPool.Dequeue() : Instantiate(dummyPrefab);
        obj.SetActive(true);

        if (lifetime > 0)
            StartCoroutine(ReturnDummyAfterTime(obj, lifetime));

        return obj;
    }

    public void ReturnDummy(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        dummyPool.Enqueue(obj);
    }

    private System.Collections.IEnumerator ReturnDummyAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        ReturnDummy(obj);
    }

    public GameObject GetVisual(string key, EnemyDataSO enemyData,  float lifetime = 1.5f)
    {
        if (!visualPool.ContainsKey(key))
            visualPool[key] = new Queue<GameObject>();

        GameObject obj;
        if (visualPool[key].Count > 0)
        {
            obj = visualPool[key].Dequeue();
        }
        else
        {
            obj = Instantiate(enemyData.modelLow);
        }
        obj.SetActive(true);

        if (lifetime > 0)
            StartCoroutine(ReturnAfterTime(key, obj, lifetime));

        return obj;
    }

    public void ReturnVisual(string key, GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);

        if (!visualPool.ContainsKey(key))
            visualPool[key] = new Queue<GameObject>();

        visualPool[key].Enqueue(obj);
    }

    private System.Collections.IEnumerator ReturnAfterTime(string key, GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        ReturnVisual(key, obj);
    }
}

