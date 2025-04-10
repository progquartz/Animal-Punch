using UnityEditor.EditorTools;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // 플레이어의 Transform 참조 (씬 내 지정)
    public Transform playerTransform;
    public EnemyPool Pool;
    // 스폰 범위 및 반납 기준 거리
    public float spawnRadius = 15f;
    public float despawnDistance = 20f;
    // 유지하고 싶은 적의 최대 수 (종류별로 관리할 수도 있음)
    public int maxEnemyCount = 10;

    // 스폰 시간 주기 (예: 매 초마다 스폰 시도)
    public float spawnInterval = 1f;
    private float spawnTimer;

    void Update()
    {
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
        // 현재 활성화된 적의 수가 최대치보다 작을 때에만 스폰
        if (FindObjectsOfType<Enemy>().Length < maxEnemyCount)
        {
            // 스폰 위치: 플레이어 주변에 랜덤으로 배치 (예시)
            Vector3 randomPos = playerTransform.position + (Random.insideUnitSphere * spawnRadius);
            randomPos.y = playerTransform.position.y;

            // 풀에서 적 오브젝트를 가져옵니다.
            // 여기서는 예를 들어 "EnemyMoving" 타입을 스폰한다고 가정
            GameObject enemyObj = Pool.GetFromPool("EnemyAttacking");
            if (enemyObj != null)
            {
                enemyObj.transform.position = randomPos;
                enemyObj.transform.rotation = Quaternion.identity;

                // 필요에 따라 EnemyDataSO를 다시 할당하여 Init을 호출해줍니다.
                Enemy enemyComponent = enemyObj.GetComponent<Enemy>();
                if (enemyComponent != null && enemyComponent.targetEnemyDataSO != null)
                {
                    enemyComponent.Init(enemyComponent.targetEnemyDataSO);
                }
            }
        }
    }
}
