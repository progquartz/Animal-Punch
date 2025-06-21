using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverScreen : MonoBehaviour
{
    public EnemyPool enemyVisualPool;
    public Transform dropContainer;
    public GameObject dummyObject;
    public float dropInterval = 0.5f;

    public void StartDroppingEnemies()
    {
        StartCoroutine(DropEnemiesRoutine());
    }

    public void TestGameOver()
    {
        GameManager.Instance.EnemyDeathCountHandler.RegisterDeath("Bear");
        GameManager.Instance.EnemyDeathCountHandler.RegisterDeath("Cow");
        GameManager.Instance.EnemyDeathCountHandler.RegisterDeath("Chicken");
        GameManager.Instance.EnemyDeathCountHandler.RegisterDeath("Crow");
        GameManager.Instance.EnemyDeathCountHandler.RegisterDeath("Duck");
        GameManager.Instance.EnemyDeathCountHandler.RegisterDeath("Elephant");
        GameManager.Instance.EnemyDeathCountHandler.RegisterDeath("Wolf");
        GameManager.Instance.EnemyDeathCountHandler.RegisterDeath("Bear");
        GameManager.Instance.EnemyDeathCountHandler.RegisterDeath("Cow");
        GameManager.Instance.EnemyDeathCountHandler.RegisterDeath("Chicken");
        GameManager.Instance.EnemyDeathCountHandler.RegisterDeath("Crow");
        GameManager.Instance.EnemyDeathCountHandler.RegisterDeath("Duck");
        GameManager.Instance.EnemyDeathCountHandler.RegisterDeath("Elephant");
        GameManager.Instance.EnemyDeathCountHandler.RegisterDeath("Wolf");
    }

    private IEnumerator DropEnemiesRoutine()
    {
        while (GameManager.Instance.EnemyDeathCountHandler.HasEnemiesLeft())
        {
            string enemyKey = GameManager.Instance.EnemyDeathCountHandler.GetRandomEnemyKey();
            if (enemyKey == null) yield break;

            Vector3 spawnPos = dropContainer.position + Random.insideUnitSphere * 2f;
            spawnPos.y = dropContainer.position.y;

            GameObject dummy = Instantiate(dummyObject);

            dummy.SetActive(false);  // 활성화 전 설정
            dummy.transform.SetParent(dropContainer, false);
            dummy.transform.position = spawnPos;
            dummy.transform.rotation = Quaternion.identity;

            Rigidbody rb = dummy.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.Sleep();
            }

            GameObject enemyVisual = DataManager.Instance.EnemyDataStorage.GetEnemyData(enemyKey).modelLow;

            Instantiate(enemyVisual, dummy.transform);
            enemyVisual.transform.localPosition = Vector3.zero;
            enemyVisual.transform.localRotation = Quaternion.identity;
            
            dummy.SetActive(true);

            yield return new WaitForSeconds(dropInterval);
        }
    }
}
