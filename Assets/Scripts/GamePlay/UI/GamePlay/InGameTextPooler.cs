using System.Collections.Generic;
using UnityEngine;

public class InGameTextPooler : SingletonBehaviour<InGameTextPooler>
{

    public GameText damageTextPrefab; 
    public int initialPoolSize = 10;

    private Queue<GameText> poolQueue = new Queue<GameText>();

    private void Awake()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameText dt = Instantiate(damageTextPrefab, transform);
            dt.gameObject.SetActive(false);
            dt.SetPool(this);
            poolQueue.Enqueue(dt);
        }
    }

    public GameText GetFromPool()
    {
        if (poolQueue.Count > 0)
        {
            GameText dt = poolQueue.Dequeue();
            dt.gameObject.SetActive(true);
            return dt;
        }
        else
        {
            // 필요시 풀 확장
            GameText dt = Instantiate(damageTextPrefab, transform);
            dt.SetPool(this);
            return dt;
        }
    }

    public void ReturnToPool(GameText dt)
    {
        dt.gameObject.SetActive(false);
        poolQueue.Enqueue(dt);
    }

    public void SpawnText(string text, Color color,  Vector3 worldPosition)
    {
        GameText dt = GetFromPool();
        dt.ShowText(text, color, worldPosition);
    }
}
