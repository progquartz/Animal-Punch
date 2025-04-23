using System;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    // 현재 게임 진행 시간
    public float GameTime;

    public float GameTimeLeft = 10f;
    public float MaxGameTime = 10f;
    public float GameTimeDecreaseRate = 1.0f;


    public bool IsTimeStop = false;
    public Action<bool> OnTimeToggle;

    public float hpIncreaseRate = 0.1f;

    private void Update()
    {
        UpdateGameTime();
        CheckGameEnd();
    }

    public void GainGameTime(float amount)
    {
        GameTimeLeft += amount;
        if(GameTimeLeft > MaxGameTime)
        {
            GameTimeLeft = MaxGameTime;
        }
    }

    private void UpdateGameTime()
    {
        GameTime += Time.deltaTime;
        GameTimeLeft -= Time.deltaTime * GameTimeDecreaseRate;
    }

    private void CheckGameEnd()
    {
        // 게임 오버 정의.
        if(GameTimeLeft < 0)
        {
            OnGameOver();
        }
    }

    private void OnGameOver()
    {
        //Logger.LogError("게임 오버!");
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
        IsTimeStop = true;
        OnTimeToggle.Invoke(true);
    }

    public void ResumeTime()
    {
        IsTimeStop = false;
        OnTimeToggle.Invoke(false);
    }
}
