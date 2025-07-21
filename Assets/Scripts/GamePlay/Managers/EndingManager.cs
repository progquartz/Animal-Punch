using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        // 게임 비활성화
        TurnOffStaticUIs();
        CameraManager.Instance.SwitchToCamera(CameraType.GameEndCamera);

        // 데이터 부분 작업
        //gameOverSpawner.TestAnimalDeathStack();
        CalculateFinalScores();
        CheckHighScore();
        CheckExp();
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

    private void CalculateFinalScores()
    {
        finalEnemyScore = GameManager.Instance.EnemyDeathCountHandler.GetDeathScore();
        finalTimeScore = Mathf.RoundToInt(GameManager.Instance.GameTime);
        finalTotalScore = finalEnemyScore + finalTimeScore;
        Debug.Log($"finalEnemyScore = {finalEnemyScore} / finalTimeScore = {finalTimeScore} / finalTotalScore = {finalTotalScore}");
    }

    private void CheckExp()
    {
        // 최종 스코어만큼 경험치를 얻는거로...
        if(finalTotalScore > 0)
        {
            GameManager.Instance.GetPlayerInfoData().QueueGainExp(finalTotalScore);
        }
    }

    private void CheckHighScore()
    {
        GameManager.Instance.GetPlayerInfoData().UpdateGameEndResult(finalTotalScore);
    }

    public void OnClickSkipButton()
    {
        if(!isSkipped)
        {
            isSkipped = true;
            gameOverSpawner.OnSkipButtonTriggered();
        }
        else
        {
            // 씬 넘어가기
            SceneLoader.Instance.LoadScene(SceneType.TitleScene);
        }
        
    }

    private void TurnOffStaticUIs()
    {
        StaticUI.SetActive(false);
        JoyStick.SetActive(false);
    }
}
