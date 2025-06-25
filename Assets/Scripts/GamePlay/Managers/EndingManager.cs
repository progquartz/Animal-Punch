using UnityEngine;

public class EndingManager : SingletonBehaviour<EndingManager>
{
    [SerializeField] private Transform GameEndingComponent;

    [SerializeField] private GameObject StaticUI;
    [SerializeField] private GameObject JoyStick;

    [SerializeField] private GameOverSpawner gameOverSpawner;
    [SerializeField] private GameOverScreenUI gameOverScreenUI;


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

        GameEndingComponent.gameObject.SetActive(true);

        gameOverSpawner.TestGameOver();
        gameOverSpawner.StartDroppingEnemies();
        float droppingTime = gameOverSpawner.GetAllSpawnTime();
        gameOverScreenUI.ShowScore(droppingTime);
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
