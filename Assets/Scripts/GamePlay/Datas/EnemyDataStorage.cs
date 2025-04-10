using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnData
{
    public string enemyKey;
    public float minSpawnTime;
    public float maxSpawnTime;
    public GameObject spawnPrefab;
}
public class EnemyDataStorage : MonoBehaviour
{
    public List<EnemySpawnData> enemyData;

    public EnemySpawnData GetRandomEnemy(float currentGameTime)
    {
        List<EnemySpawnData> validEnemies = enemyData.FindAll(e => currentGameTime >= e.minSpawnTime && currentGameTime <= e.maxSpawnTime);
        if (validEnemies.Count == 0)
        {
            return null;
        }
        return validEnemies[Random.Range(0, validEnemies.Count)];
    }

    public EnemySpawnData GetEnemyOnKey(string enemyKey)
    {
        List<EnemySpawnData> validEnemies = enemyData.FindAll (e=> e.enemyKey == enemyKey);

        if(validEnemies.Count == 0)
        {
            return null; 
        }

        if(validEnemies.Count == 1)
        {
            return validEnemies[0];
        }

        return validEnemies[Random.Range(0, validEnemies.Count)];
    }
}
