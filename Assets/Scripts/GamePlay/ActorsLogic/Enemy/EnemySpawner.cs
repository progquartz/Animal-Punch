using UnityEditor.EditorTools;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // �÷��̾��� Transform ���� (�� �� ����)
    public Transform playerTransform;
    public EnemyPool Pool;
    // ���� ���� �� �ݳ� ���� �Ÿ�
    public float spawnRadius = 15f;
    public float despawnDistance = 20f;
    // �����ϰ� ���� ���� �ִ� �� (�������� ������ ���� ����)
    public int maxEnemyCount = 10;

    // ���� �ð� �ֱ� (��: �� �ʸ��� ���� �õ�)
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

        // �� ���� �����ϴ� ��� ���� �˻��ؼ� �ָ� �ִ� ��� Ǯ�� �ݳ�
        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            if (Vector3.Distance(enemy.transform.position, playerTransform.position) > despawnDistance)
            {
                // enemy�� Ÿ��(�̸�)�� Ű�� �Ͽ� Ǯ�� �ݳ�
                string poolKey = enemy.name.Replace("(Clone)", "").Trim();
                Pool.ReturnToPool(poolKey, enemy.gameObject);
            }
        }
    }

    void TrySpawnEnemy()
    {
        // ���� Ȱ��ȭ�� ���� ���� �ִ�ġ���� ���� ������ ����
        if (FindObjectsOfType<Enemy>().Length < maxEnemyCount)
        {
            // ���� ��ġ: �÷��̾� �ֺ��� �������� ��ġ (����)
            Vector3 randomPos = playerTransform.position + (Random.insideUnitSphere * spawnRadius);
            randomPos.y = playerTransform.position.y;

            // Ǯ���� �� ������Ʈ�� �����ɴϴ�.
            // ���⼭�� ���� ��� "EnemyMoving" Ÿ���� �����Ѵٰ� ����
            GameObject enemyObj = Pool.GetFromPool("EnemyAttacking");
            if (enemyObj != null)
            {
                enemyObj.transform.position = randomPos;
                enemyObj.transform.rotation = Quaternion.identity;

                // �ʿ信 ���� EnemyDataSO�� �ٽ� �Ҵ��Ͽ� Init�� ȣ�����ݴϴ�.
                Enemy enemyComponent = enemyObj.GetComponent<Enemy>();
                if (enemyComponent != null && enemyComponent.targetEnemyDataSO != null)
                {
                    enemyComponent.Init(enemyComponent.targetEnemyDataSO);
                }
            }
        }
    }
}
