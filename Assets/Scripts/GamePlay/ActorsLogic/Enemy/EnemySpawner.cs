using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public Transform playerTransform;
    public Transform EnemyParentTransform;
    public Dictionary<string, int> EnemySpawnMaximum;

    public EnemyPool Pool;
    // 스폰 범위 및 반납 기준 거리
    private float spawnRadius = 10f;
    private float despawnDistance = 100f;
    // 유지하고 싶은 적의 최대 수 (종류별로 관리할 수도 있음)
    private int maxEnemyCount = 10;

    public float spawnInterval = 1f;
    private float spawnTimer;

    private float despawnDistanceXZ = 50f;
    private float despawnDistanceY = 10f;
    private float despawnDistanceXYZ = 50f;

    public void Init()
    {
        EnemyParentTransform = GameObject.Find("EnemyParent").transform;
        EnemySpawnMaximum = new Dictionary<string, int>();
        foreach(EnemyDataSO data in  DataManager.Instance.EnemyDataStorage.enemyDataList.Values)
        {
            EnemySpawnMaximum.Add(data.ActorKey, data.MaxSpawnCount);
        }
    }

    void Update()
    {
        if (GameManager.Instance.IsTimeStop) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0;
            TrySpawnEnemyWave();
        }

        foreach(Transform child in EnemyParentTransform)
        {
            Enemy enemy;
            if(child.TryGetComponent<Enemy>(out enemy))
            {
                if (!enemy.IsInPool && IsOutOfDespawnDistance(enemy.transform.transform.position, playerTransform.position))
                {
                    // enemy의 타입(이름)을 키로 하여 풀로 반납
                    string poolKey = enemy.targetEnemyDataSO.ActorKey;
                    Pool.ReturnToPool(poolKey, enemy.gameObject);
                }
            }
        }
    }


    private bool IsOutOfDespawnDistance(Vector3 enemyPosition, Vector3 playerPosition)
    {
        float dx = enemyPosition.x - playerPosition.x;
        float dy = enemyPosition.y - playerPosition.y;
        float dz = enemyPosition.z - playerPosition.z;

        float distanceXZ = Mathf.Sqrt(dx * dx + dz * dz);
        float distanceY = Mathf.Abs(dy);

        float distance = (enemyPosition - playerPosition).magnitude;

        if (distanceXZ >= despawnDistanceXZ || distanceY >= despawnDistanceY || distance >= despawnDistance)
        {
            return true;
        }
        return false;
    }

    
    private void TrySpawnEnemyWave()
    {
        string enemyKey = "CowTest"; // 이걸 받아와서 사용하게 만들기...
        // 개체 조절
        if(IsKeyValidToSpawn(enemyKey))
        {
            TrySpawnEnemy(enemyKey);
        }
    }

    private bool IsKeyValidToSpawn(string key)
    {
        return (GetEnemyCount() < maxEnemyCount) && Pool.GetEnemyCount(key) < EnemySpawnMaximum[key];
    }

    /// <summary>
    /// 현재 코드 실험 중. 효율성이 떨어지나, 사용에는 문제 없음.
    /// </summary>
    /// <returns></returns>
    private int GetEnemyCount()
    {
        int enemyCountOnHierarchy = 0;
        int enemyCountOnPool = Pool.GetAllEnemyCount();

        foreach (Transform child in EnemyParentTransform)
        {
            if (child.gameObject.activeInHierarchy)
            {
                enemyCountOnHierarchy++;
            }
        }

        if(enemyCountOnHierarchy != enemyCountOnPool)
        {
            Debug.LogWarning($"Pool에서 counting되는 enemyCount인 {enemyCountOnPool}개와, Hierarchy에서 Counting되는 enemyCount인 {enemyCountOnHierarchy}개가 서로 다릅니다. ");
            return enemyCountOnHierarchy;
        }
        return enemyCountOnPool;    
    }

    private void TrySpawnEnemy(string key)
    {
        Vector3 randomPos = playerTransform.position + (Random.insideUnitSphere * spawnRadius);
        randomPos.y = 0f;

        // 태그 기반이 아닌, Data에서 조건에 맞는 랜덤한 적 key 가지고 오기
        GameObject enemyObj = Pool.GetFromPool(key);
        enemyObj.transform.position = randomPos;
        enemyObj.transform.rotation = Quaternion.identity;

        if (enemyObj != null)
        {
            Enemy enemyComponent = enemyObj.GetComponent<Enemy>();
            if (enemyComponent != null && enemyComponent.targetEnemyDataSO != null)
            {
                enemyComponent.Init(DataManager.Instance.EnemyDataStorage.GetEnemyData(key));
            }
        }
    }

    
}
