using UnityEditor.EditorTools;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public Transform playerTransform;
    public Transform EnemyParentTransform;

    public EnemyPool Pool;
    // 스폰 범위 및 반납 기준 거리
    private float spawnRadius = 10f;
    private float despawnDistance = 100f;
    // 유지하고 싶은 적의 최대 수 (종류별로 관리할 수도 있음)
    private int maxEnemyCount = 10;

    public float spawnInterval = 1f;
    private float spawnTimer;

    private float despawnDistanceXZ = 100f;
    private float despawnDistanceY = 10f;

    public void Init()
    {
        EnemyParentTransform = GameObject.Find("EnemyParent").transform;
    }

    void Update()
    {
        if (GameManager.Instance.IsTimeStop) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0;
            TrySpawnEnemy();
        }

        foreach(Transform child in EnemyParentTransform)
        {
            Enemy enemy;
            if(child.TryGetComponent<Enemy>(out enemy))
            {
                if (!enemy.IsInPool && IsOutOfDespawnDistance(enemy.transform.transform.position, playerTransform.position))
                {
                    // enemy의 타입(이름)을 키로 하여 풀로 반납
                    string poolKey = enemy.name.Replace("(Clone)", "").Trim();
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

        if (distanceXZ >= despawnDistanceXZ || distanceY >= despawnDistanceY)
        {
            return true;
        }
        return false;
    }

    void TrySpawnEnemy()
    {
        int enemyCount = 0;
        foreach(Transform child in EnemyParentTransform)
        {
            if(child.gameObject.activeInHierarchy)
            {
                enemyCount++;
            }
        }

        // 개체 조절
        if (enemyCount < maxEnemyCount)
        {
            Vector3 randomPos = playerTransform.position + (Random.insideUnitSphere * spawnRadius);
            randomPos.y = 0f;

            // 태그 기반이 아닌, Data에서 조건에 맞는 랜덤한 적 key 가지고 오기
            GameObject enemyObj = Pool.GetFromPool("EnemyAttacking");
            enemyObj.transform.position = randomPos;
            enemyObj.transform.rotation = Quaternion.identity;

            if (enemyObj != null)
            {
                Enemy enemyComponent = enemyObj.GetComponent<Enemy>();
                if (enemyComponent != null && enemyComponent.targetEnemyDataSO != null)
                {
                    enemyComponent.Init(enemyComponent.targetEnemyDataSO);
                }
            }
        }
    }
}
