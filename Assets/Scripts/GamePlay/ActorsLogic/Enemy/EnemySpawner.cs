using UnityEditor.EditorTools;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public Transform playerTransform;
    public EnemyPool Pool;
    // 스폰 범위 및 반납 기준 거리
    private float spawnRadius = 10f;
    private float despawnDistance = 100f;
    // 유지하고 싶은 적의 최대 수 (종류별로 관리할 수도 있음)
    private int maxEnemyCount = 10;

    public float spawnInterval = 1f;
    private float spawnTimer;

    void Update()
    {
        if (GameManager.Instance.IsTimeStop) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0;
            TrySpawnEnemy();
        }

        // 씬 내에 존재하는 모든 적을 검사해서 멀리 있는 경우 풀로 반납
        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            if (Vector3.Distance(enemy.transform.position, playerTransform.position) > despawnDistance)
            {
                // enemy의 타입(이름)을 키로 하여 풀로 반납
                string poolKey = enemy.name.Replace("(Clone)", "").Trim();
                Pool.ReturnToPool(poolKey, enemy.gameObject);
            }
        }
    }

    void TrySpawnEnemy()
    {
        // 개체 조절
        if (FindObjectsOfType<Enemy>().Length < maxEnemyCount)
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
