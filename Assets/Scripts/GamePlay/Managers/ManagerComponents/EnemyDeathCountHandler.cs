using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathCountHandler
{
    private Dictionary<string, int> deathCounts = new Dictionary<string, int>();
    private List<string> availableKeys = new List<string>();

    public void RegisterDeath(string enemyKey)
    {
        Debug.Log($"{enemyKey}주금");
        if (!deathCounts.ContainsKey(enemyKey))
        {
            deathCounts[enemyKey] = 1;
            availableKeys.Add(enemyKey);
        }
        else
        {
            deathCounts[enemyKey]++;
            if (deathCounts[enemyKey] == 1)
                availableKeys.Add(enemyKey);
        }
    }

    public string GetRandomEnemyKey()
    {
        if (availableKeys.Count == 0) return null;

        int index = Random.Range(0, availableKeys.Count);
        string randomKey = availableKeys[index];

        deathCounts[randomKey]--;

        // 만약 카운트가 0이 되면 리스트에서 제거
        if (deathCounts[randomKey] <= 0)
        {
            availableKeys.RemoveAt(index);
        }

        return randomKey;
    }

    public bool HasEnemiesLeft()
    {
        return availableKeys.Count > 0;
    }

    public int GetTotalDeathCount()
    {
        int total = 0;
        foreach (var count in deathCounts.Values)
        {
            total += count;
        }
        return total;
    }
}
