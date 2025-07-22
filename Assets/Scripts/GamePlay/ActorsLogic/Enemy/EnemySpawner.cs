using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    private int maxEnemyCount = 100;

    public bool IsPoolingReady = false;

    private Dictionary<string, float> enemySpawnTimers = new Dictionary<string, float>();
    private Dictionary<string, Coroutine> spawningCoroutines = new Dictionary<string, Coroutine>();


    private List<string> EnemyNeedToSpawnList = new List<string>();
    private List<string> EnemySpawnRightNowList = new List<string>();

    public void Init()
    {
        Debug.Log("EnemySpawner Init");
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
        if (!GameManager.Instance.IsGameStarted ||  GameManager.Instance.IsGamePaused || GameManager.Instance.IsGameOver) return;

        HandleImmediateSpawn();
        HandleIntervalSpawn();

        CheckDespawnEnemies();
    }



    public async Task PreloadAllEnemyObjects()
    {
        try
        {
            Init();
            var enemyDataList = DataManager.Instance.EnemyDataStorage.enemyDataList;

            foreach (var enemyDataPair in enemyDataList)
            {
                string enemyKey = enemyDataPair.Key;
                EnemyDataSO enemyData = enemyDataPair.Value;

                int spawnCount = enemyDataPair.Value.MaxSpawnCount >= 10 ? 10 : enemyDataPair.Value.MaxSpawnCount;
                Pool.InitializePool(enemyKey, spawnCount);
                await Task.Yield();
            }

            IsPoolingReady = true;
            Debug.Log("EnemySpawner: Preload completed.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"EnemySpawner preload failed: {e.Message}");
            IsPoolingReady = false;
        }
    }



    private void HandleIntervalSpawn()
    {
        
        
        CheckIntervalTime();

        if (EnemyNeedToSpawnList.Count > 0 && GetEnemyCount() < maxEnemyCount)
        {
            int randomIndex = Random.Range(0, EnemyNeedToSpawnList.Count);
            string randomEnemyKey = EnemyNeedToSpawnList[randomIndex];
            EnemyNeedToSpawnList.RemoveAt(randomIndex);

            SpawnEnemies(randomEnemyKey);
        }
    }

    private void CheckIntervalTime()
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
    }

    private bool IsWithinSpawnTime(EnemyDataSO enemyData, float gameTime)
    {
        bool minTimeValid = enemyData.MinSpawnTime == -1f || gameTime >= enemyData.MinSpawnTime;
        bool maxTimeValid = enemyData.MaxSpawnTime == -1f || gameTime <= enemyData.MaxSpawnTime;

        if((minTimeValid && maxTimeValid) == false)
        {
            Debug.Log($"enemyName = {enemyData.name} | currentTime = {gameTime} | minTimeValid = {enemyData.MinSpawnTime} | maxTimeValid = {enemyData.MaxSpawnTime}");
        }
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
    /// 현재 풀 상에 존재하는 적의 개수 반환
    /// </summary>
    /// <returns></returns>
    private int GetEnemyCount()
    {
        int enemyCountOnPool = Pool.GetAllEnemyCount();
        return enemyCountOnPool;    
    }

    public void InitializeAllEnemyPools(int defaultPoolSize = 10)
    {
        var enemyDataList = DataManager.Instance.EnemyDataStorage.enemyDataList;

        foreach (var enemyDataPair in enemyDataList)
        {
            string enemyKey = enemyDataPair.Key;
            EnemyDataSO enemyData = enemyDataPair.Value;

            // EnemyPool을 통해 적 타입 초기화
            Pool.InitializePool(enemyKey, defaultPoolSize);
        }
    }


    private void SpawnEnemies(string key)
    {
        if (spawningCoroutines.ContainsKey(key))
        {
            return;
        }

        spawningCoroutines.Add(key, null);   // 먼저 추가
        Coroutine spawnRoutine = StartCoroutine(SpawnEnemiesCoroutine(key));
    }

    private IEnumerator SpawnEnemiesCoroutine(string key, int countPerFrame = 3)
    {
        int spawnCount = DataManager.Instance.EnemyDataStorage.enemyDataList[key].SpawnCount;
        for (int i = 0; i < spawnCount; i++)
        {
            if (!IsKeyValidToSpawn(key)) break;

            float randomRadius = Random.Range(minSpawnRadius, maxSpawnRadius);
            Vector3 direction = Random.onUnitSphere;
            
            Vector3 randomPos = playerTransform.position + direction * randomRadius;
            randomPos.y = 0f;

            float randomRotationY = Random.Range(0f, 360f);
            Quaternion randomRotation = Quaternion.Euler(0f, randomRotationY, 0f);
            

            GameObject enemyObj = Pool.GetFromPool(key, EnemyParentTransform, randomPos, randomRotation);
            Enemy enemyComponent = enemyObj.GetComponent<Enemy>();

            if (enemyComponent != null && enemyComponent.targetEnemyDataSO != null)
            {
                enemyComponent.Init(DataManager.Instance.EnemyDataStorage.GetEnemyData(key));
            }

            // 일정 개수마다 다음 프레임으로 넘김
            if ((i + 1) % countPerFrame == 0)
            {
                yield return null; 
            }
        }
        spawningCoroutines.Remove(key);
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

    public void DeSpawnAllEnemies()
    {
        foreach(Transform child in EnemyParentTransform)
        {
            Enemy enemy;
            if (child.TryGetComponent<Enemy>(out enemy))
            {
                // 풀에 없으니까 소환된거임...
                if (!enemy.IsInPool)
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
