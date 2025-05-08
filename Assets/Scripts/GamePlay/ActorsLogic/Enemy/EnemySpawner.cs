using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public Transform playerTransform;
    public Transform EnemyParentTransform;
    public Dictionary<string, int> EnemySpawnMaximum;

    public EnemyPool Pool;
    
    // 스폰 범위 및 반납 기준 거리
    private float maxSpawnRadius = 30f;
    private float minSpawnRadius = 10f;

    private float despawnDistanceXZ = 80f;
    private float despawnDistanceY = 10f;
    private float despawnDistance = 80f;


    // 유지하고 싶은 적의 최대 수 (종류별로 관리할 수도 있음)
    private int maxEnemyCount = 30;
    
    private Dictionary<string, float> enemySpawnTimers = new Dictionary<string, float>();

    private List<string> EnemyNeedToSpawnList = new List<string>();
    private List<string> EnemySpawnRightNowList = new List<string>();




    public void Init()
    {
        EnemyParentTransform = GameObject.Find("EnemyParent").transform;
        EnemySpawnMaximum = new Dictionary<string, int>();

        foreach (EnemyDataSO data in DataManager.Instance.EnemyDataStorage.enemyDataList.Values)
        {
            EnemySpawnMaximum.Add(data.ActorKey, data.MaxSpawnCount);
            enemySpawnTimers.Add(data.ActorKey, data.SpawnInterval);

            if (data.NeedSpawnRightNow)
            {
                EnemySpawnRightNowList.Add(data.ActorKey);
            }
        }
    }

    void Update()
    {
        if (GameManager.Instance.IsTimeStop) return;

        HandleImmediateSpawn();
        HandleIntervalSpawn();

        CheckDespawnEnemies();
    }

    private void HandleIntervalSpawn()
    {
        float currentGameTime = GameManager.Instance.GameTime;

        // Dictionary의 키 목록을 별도 리스트로 복사하여 순회하는데, 이 부분은 dictionary의 값이 변경되어 오류가 나는 부분을 고치기 위함임.
        List<string> enemyKeys = new List<string>(enemySpawnTimers.Keys);

        foreach (var enemyKey in enemyKeys)
        {
            EnemyDataSO enemyData = DataManager.Instance.EnemyDataStorage.enemyDataList[enemyKey];

            enemySpawnTimers[enemyKey] -= Time.deltaTime;

            if (enemySpawnTimers[enemyKey] <= 0f)
            {
                bool isTimeValid = IsWithinSpawnTime(enemyData, currentGameTime);

                if (isTimeValid)
                {
                    EnemyNeedToSpawnList.Add(enemyKey);
                }

                enemySpawnTimers[enemyKey] = enemyData.SpawnInterval;
            }
        }

        if (EnemyNeedToSpawnList.Count > 0 && GetEnemyCount() < maxEnemyCount)
        {
            int randomIndex = Random.Range(0, EnemyNeedToSpawnList.Count);
            string randomEnemyKey = EnemyNeedToSpawnList[randomIndex];
            EnemyNeedToSpawnList.RemoveAt(randomIndex);

            SpawnEnemies(randomEnemyKey);
        }
    }

    private bool IsWithinSpawnTime(EnemyDataSO enemyData, float gameTime)
    {
        bool minTimeValid = enemyData.MinSpawnTime == -1f || gameTime >= enemyData.MinSpawnTime;
        bool maxTimeValid = enemyData.MaxSpawnTime == -1f || gameTime <= enemyData.MaxSpawnTime;

        return minTimeValid && maxTimeValid;
    }


    private void HandleImmediateSpawn()
    {
        while (EnemySpawnRightNowList.Count > 0)
        {
            string key = EnemySpawnRightNowList[0];
            EnemySpawnRightNowList.RemoveAt(0);
            SpawnEnemies(key);
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

    private void SpawnEnemies(string key)
    {
        int spawnCount = DataManager.Instance.EnemyDataStorage.enemyDataList[key].SpawnCount;

        for (int i = 0; i < spawnCount; i++)
        {
            if (!IsKeyValidToSpawn(key)) break;

            float randomRadius = Random.Range(minSpawnRadius, maxSpawnRadius);
            Vector3 direction = Random.onUnitSphere; // 방향만 랜덤, 길이는 1
            Vector3 randomPos = playerTransform.position + direction * randomRadius;
            randomPos.y = 0f;

            GameObject enemyObj = Pool.GetFromPool(key);
            if (enemyObj != null)
            {
                enemyObj.transform.position = randomPos;
                enemyObj.transform.rotation = Quaternion.identity;
                Logger.Log($"{key}의 적을 받아와서 {randomPos}에 배치합니다.");

                Enemy enemyComponent = enemyObj.GetComponent<Enemy>();
                if (enemyComponent != null && enemyComponent.targetEnemyDataSO != null)
                {
                    enemyComponent.Init(DataManager.Instance.EnemyDataStorage.GetEnemyData(key));
                }
            }
        }
    }


    private void CheckDespawnEnemies()
    {
        foreach (Transform child in EnemyParentTransform)
        {
            Enemy enemy;
            if (child.TryGetComponent<Enemy>(out enemy))
            {
                // 풀에 없으니까 소환된거임...
                if (!enemy.IsInPool && IsOutOfDespawnDistance(enemy.transform.position, playerTransform.position))
                {
                    string poolKey = enemy.targetEnemyDataSO.ActorKey;
                    Pool.ReturnToPool(poolKey, enemy.gameObject);
                }
            }
        }
    }

    private bool IsOutOfDespawnDistance(Vector3 enemyPosition, Vector3 playerPosition)
    {
        float dx = enemyPosition.x - playerPosition.x;
        float dz = enemyPosition.z - playerPosition.z;
        float dy = Mathf.Abs(enemyPosition.y - playerPosition.y);

        float distanceXZ = Mathf.Sqrt(dx * dx + dz * dz);
        float distance = (enemyPosition - playerPosition).magnitude;

        if (distanceXZ >= despawnDistanceXZ || dy >= despawnDistanceY || distance >= despawnDistance)
        {
            return true;
        }
        return false;
    }


}
