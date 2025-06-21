using UnityEngine;

public class EndingManager : SingletonBehaviour<EndingManager>
{
    [SerializeField] private GameOverScreen gameOverScreen;

    protected override void Init()
    {
        IsDestroyOnLoad = true;
        base.Init();
    }

    public void OnGameOver()
    {
        gameOverScreen.TestGameOver();
        gameOverScreen.StartDroppingEnemies();
    }


}

