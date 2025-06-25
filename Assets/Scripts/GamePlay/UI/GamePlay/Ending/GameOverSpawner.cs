using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverSpawner : MonoBehaviour
{
    public Transform dropContainer;
    public Transform dropContainerPos;
    public GameObject dummyObject;
    public bool isSkipped = false;
    [SerializeField] private GameOverObjectPool pool;
    public readonly float dropInterval = 0.03f;

    public void StartDroppingEnemies()
    {
        dropContainer.position = dropContainerPos.position;
        StartCoroutine(DropEnemiesRoutine());
    }

    public void OnSkipButtonTriggered()
    {
        isSkipped = true;
        while (GameManager.Instance.EnemyDeathCountHandler.HasEnemiesLeft())
        {
            string enemyKey = GameManager.Instance.EnemyDeathCountHandler.GetRandomEnemyKey();
            EnemyDataSO enemyData = DataManager.Instance.EnemyDataStorage.GetEnemyData(enemyKey);
            EndingManager.Instance.HandleEndingScreenUI(enemyData.DropItemData.ExpAmount, true);

        }
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

    public float GetAllSpawnTime()
    {
        return dropInterval * GameManager.Instance.EnemyDeathCountHandler.GetTotalDeathCount();
    }

    /*

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
            dummy.transform.rotation = Random.rotation;

            Rigidbody rb = dummy.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.Sleep();
            }


            EnemyDataSO enemyData = DataManager.Instance.EnemyDataStorage.GetEnemyData(enemyKey);
            EndingManager.Instance.HandleEndingScreenUI(enemyData.DropItemData.ExpAmount);

            GameObject enemyVisual = enemyData.modelLow;

            Instantiate(enemyVisual, dummy.transform);
            enemyVisual.transform.localPosition = Vector3.zero;
            enemyVisual.transform.localRotation = Quaternion.identity;

            dummy.SetActive(true);

            yield return new WaitForSeconds(dropInterval);
        }
    }
    */

    private IEnumerator DropEnemiesRoutine()
    {
        while (GameManager.Instance.EnemyDeathCountHandler.HasEnemiesLeft() || !isSkipped)
        {
            string enemyKey = GameManager.Instance.EnemyDeathCountHandler.GetRandomEnemyKey();
            EnemyDataSO enemyData = DataManager.Instance.EnemyDataStorage.GetEnemyData(enemyKey);
            EndingManager.Instance.HandleEndingScreenUI(enemyData.DropItemData.ExpAmount);

            if (enemyKey == null) yield break;


            Vector3 spawnPos = dropContainer.position + Random.insideUnitSphere * 2f;
            spawnPos.y = dropContainer.position.y;

            GameObject dummy = pool.GetDummy();

            dummy.SetActive(false);  // 활성화 전 설정
            dummy.transform.SetParent(dropContainer, false);
            dummy.transform.position = spawnPos;
            dummy.transform.rotation = Random.rotation;

            Rigidbody rb = dummy.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.Sleep();
            }


            GameObject enemyVisual = pool.GetVisual(enemyKey, enemyData);
            enemyVisual.transform.SetParent(dummy.transform);
            enemyVisual.transform.localPosition = Vector3.zero;
            enemyVisual.transform.localRotation = Quaternion.identity;


            dummy.SetActive(true);

            yield return new WaitForSeconds(dropInterval);
        }
    }
}
