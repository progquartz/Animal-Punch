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
    public int finalTotalScore = 0;
    public int finalTimeScore = 0;
    public int finalEnemyScore = 0;
    public List<LootingData> lootingData;


    protected override void Init()
    {
        IsDestroyOnLoad = true;
        GameEndingComponent.gameObject.SetActive(false);
        base.Init();
    }

    public void OnGameOver()
    {
        TurnOffStaticUIs();

        CameraManager.Instance.SwitchToCamera(CameraType.GameEndCamera);


        //

        // 데이터 부분 작업
        CalculateFinalScores();
        float droppingTime = gameOverSpawner.GetAllSpawnTime();
        Debug.Log("Test1");
        lootingData = BoxInventoryManager.Instance.CalculateGameEndingLoots(finalTotalScore);
        foreach (LootingData data in lootingData)
        {
            Debug.Log($"{data.rankType.ToString()}랭크를 {data.count} 개 드랍합니다.");
        }




        GameEndingComponent.gameObject.SetActive(true);

        gameOverSpawner.TestGameOver();
        gameOverSpawner.StartDroppingEnemies();
        
        gameOverScreenUI.ShowScore(droppingTime);

        
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
        gameOverSpawner.OnSkipButtonTriggered();
        gameOverScreenUI.OnSkipButtonTriggered();
    }

    private void TurnOffStaticUIs()
    {
        StaticUI.SetActive(false);
        JoyStick.SetActive(false);
    }
}
