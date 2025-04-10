using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    public float gameTime;
    public float hpIncreaseRate = 0.1f;

    private void Update()
    {
        gameTime += Time.deltaTime;
    }

    /// <summary>
    /// 현재 게임 진행 시간에 따른 HP 배율을 계산합니다.
    /// </summary>
    public float GetHPMultiplier()
    {
        return 1f + gameTime * hpIncreaseRate;
    }
}
