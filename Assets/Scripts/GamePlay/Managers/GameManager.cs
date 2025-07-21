using System;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    
    public VideoSettingComponent VideoSetting = new VideoSettingComponent();
    public HealthRatioComponent EnemyHealthRatio = new HealthRatioComponent();
    public EnemyDeathCountHandler EnemyDeathCountHandler = new EnemyDeathCountHandler();
    
    [SerializeField] private PlayerInfoDatas _playerInfoDatas;
    

    // 현재 게임 진행 시간
    public float GameTime;

    public float InitialGameTimeLeft = 10f;
    public float GameTimeLeft = 10f;
    public float MaxGameTime = 10f;
    public float GameTimeDecreaseRate = 1.0f;


    // 타이틀 씬 방문 여부.
    public bool isFirstTimeInTitle = true;
    // 게임 상태
    public bool IsGameStarted = false;
    public bool IsGamePaused = false;
    public bool IsGameOver = false;
    public Action<bool> OnTimeToggle;
    public Action OnQuitGameScene;

    public float hpIncreaseRate = 0.1f;


    protected override void Init()
    {
        base.Init();
        Debug.Log("GameManager Init");
    }


    private void Update()
    {
        if(SceneLoader.Instance.CurrentScene == "Scenes/GameScene")
        {
            //Debug.Log($"isGameStarted = {IsGameStarted} / isgamePaused = {IsGamePaused} / isGameOver = {IsGameOver}");
            if (IsGameStarted && !IsGamePaused && !IsGameOver)
            {
                UpdateGameTime();
                UpdateGameRatios();
                CheckGameEnd();
            }
            else
            {
                // 타이틀 씬 및 다른 곳에서는 playerInfoData 수정 외에는 다른 행동 금지.
            }
        }
    }

    private void UpdateGameRatios()
    {
        EnemyHealthRatio.UpdateRatio();
    }

    private void UpdateGameTime()
    {
        GameTime += Time.deltaTime;
        GameTimeLeft -= Time.deltaTime * GameTimeDecreaseRate;
    }


    public void StartGameState()
    {
        if(IsGameStarted)
        {
            Logger.LogWarning("이미 게임을 실행 중인데, 또 다시 호출하려 합니다.");
        }
        IsGameStarted = true;
        IsGamePaused = false;
        IsGameOver = false;
        GameTime = 0f;
        GameTimeLeft = InitialGameTimeLeft;
        MaxGameTime = InitialGameTimeLeft;
        //UIManager.Instance.StartGameState();
        //Player.Instance.StartGameState();
    }

    public void OnNaturalGameOver()
    {
        EndGameState();
    }

    public void OnClickQuitGameOver()
    {
        EndGameState(true);
        SceneLoader.Instance.LoadScene(SceneType.TitleScene);
    }

    public void EndGameState(bool isFinishedOnForce = false)
    {
        IsGameOver = true;
        IsGamePaused = true;
        IsGameStarted = false;
        GameTimeLeft = InitialGameTimeLeft;
        MaxGameTime = InitialGameTimeLeft;
        OnQuitGameScene?.Invoke();

        if (!isFinishedOnForce)
        {
            EndingManager.Instance.OnGameOver();
        }

        MapManager.Instance.EnemySpawner.DeSpawnAllEnemies();
        Player.Instance.gameObject.SetActive(false);

        // 풀 빼기
        MapObjectPool.Instance.ClearAllPools();
        MapManager.Instance.EnemySpawner.Pool.ClearAllPools();
    }



    private void CheckGameEnd()
    {
        // 게임 오버 정의.
        if (GameTimeLeft < 0 && !IsGameOver)
        {
            OnNaturalGameOver();
        }
    }

    public void GainGameTime(float amount)
    {
        if (GameTime <= 0f) return;
        GameTimeLeft += amount;
        if(GameTimeLeft > MaxGameTime)
        {
            GameTimeLeft = MaxGameTime;
        }
    }

    public float GetHealthRatio()
    {
        return EnemyHealthRatio.Ratio;
    }


    /// <summary>
    /// 현재 게임 진행 시간에 따른 HP 배율을 계산합니다.
    /// </summary>
    public float GetHPMultiplier()
    {
        return 1f + GameTime * hpIncreaseRate;
    }

    public void StopTime()
    {
        if(!IsGamePaused)
        {
            IsGamePaused = true;
            OnTimeToggle.Invoke(true);
        }
    }

    public void ResumeTime()
    {
        if(IsGamePaused)
        {
            IsGamePaused = false;
            OnTimeToggle.Invoke(false);
        }
        
    }

    public PlayerInfoDatas GetPlayerInfoData()
    {
        if (_playerInfoDatas == null)
        {
            _playerInfoDatas = gameObject.GetComponent<PlayerInfoDatas>();
            if (_playerInfoDatas == null)
            {
                _playerInfoDatas = gameObject.AddComponent<PlayerInfoDatas>();
                _playerInfoDatas.Init();
            }
        }
        return _playerInfoDatas;
    }


}
