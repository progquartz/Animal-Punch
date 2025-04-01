using System.Collections.Generic;
using UnityEngine;

public class DamageTextPooler : MonoBehaviour
{
    public static DamageTextPooler Instance { get; private set; }

    public DamageText damageTextPrefab; 
    public int initialPoolSize = 10;

    private Queue<DamageText> poolQueue = new Queue<DamageText>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        for (int i = 0; i < initialPoolSize; i++)
        {
            DamageText dt = Instantiate(damageTextPrefab, transform);
            dt.gameObject.SetActive(false);
            dt.SetPool(this);
            poolQueue.Enqueue(dt);
        }
    }

    public DamageText GetFromPool()
    {
        if (poolQueue.Count > 0)
        {
            DamageText dt = poolQueue.Dequeue();
            dt.gameObject.SetActive(true);
            return dt;
        }
        else
        {
            // 필요시 풀 확장
            DamageText dt = Instantiate(damageTextPrefab, transform);
            dt.SetPool(this);
            return dt;
        }
    }

    public void ReturnToPool(DamageText dt)
    {
        dt.gameObject.SetActive(false);
        poolQueue.Enqueue(dt);
    }

    public void SpawnDamageText(float damage, Vector3 worldPosition)
    {
        DamageText dt = GetFromPool();
        dt.ShowDamage(damage, worldPosition);
    }
}
