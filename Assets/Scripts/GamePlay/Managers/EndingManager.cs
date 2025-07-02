using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EndingManager : SingletonBehaviour<EndingManager>
{
    [SerializeField] private Transform GameEndingComponent;

    [SerializeField] private GameObject StaticUI;
    [SerializeField] private GameObject JoyStick;

    [SerializeField] private GameOverSpawner gameOverSpawner;
    [SerializeField] private GameOverScreenUI gameOverScreenUI;
    [SerializeField] private GameOverLootingUI gameOverLootingUI;
    public int finalTotalScore = 0;
    public int finalTimeScore = 0;
    public int finalEnemyScore = 0;
    public List<LootingData> lootingData;

    public bool isSkipped = false;


    protected override void Init()
    {
        IsDestroyOnLoad = true;
        GameEndingComponent.gameObject.SetActive(false);
        base.Init();
    }

    public void OnGameOver()
    {
        GameManager.Instance.EndGameState();

        // 게임 비활성화
        TurnOffStaticUIs();
        CameraManager.Instance.SwitchToCamera(CameraType.GameEndCamera);

        // 데이터 부분 작업
        gameOverSpawner.TestAnimalDeathStack();
        CalculateFinalScores();
        float droppingTime = gameOverSpawner.GetAllSpawnTime();
        lootingData = InventoryManager.Instance.CalculateGameEndingLoots(finalTotalScore);
        InventoryManager.Instance.GetLoot(lootingData);


        // UI 작업
        GameEndingComponent.gameObject.SetActive(true);
        gameOverSpawner.StartDroppingEnemies();
        
        gameOverScreenUI.ShowScore(droppingTime);

        
    }

    public void ShowLoot()
    {
        gameOverLootingUI.ShowLoot(finalTotalScore, lootingData);
    }


    public void OnGameOverDummy()
    {
        GameManager.Instance.OnGameOver();
    }

    public void HandleEndingScreenUI(int score, bool isSkipped = false)
    {
        if(!isSkipped)
        {
            gameOverScreenUI.AddEnemyScore(score);
        }
        else
        {
            gameOverScreenUI.AddEnemyScoreSkipped(score);
        }
    }

    public void CalculateFinalScores()
    {
        finalEnemyScore = GameManager.Instance.EnemyDeathCountHandler.GetDeathScore();
        finalTimeScore = Mathf.RoundToInt(GameManager.Instance.GameTime);
        finalTotalScore= finalEnemyScore + finalTimeScore;
    }
    public void OnClickSkipButton()
    {
        isSkipped = true;
        gameOverSpawner.OnSkipButtonTriggered();
    }

    private void TurnOffStaticUIs()
    {
        StaticUI.SetActive(false);
        JoyStick.SetActive(false);
    }
}
