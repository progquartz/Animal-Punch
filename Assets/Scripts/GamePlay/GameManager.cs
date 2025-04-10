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
    /// ���� ���� ���� �ð��� ���� HP ������ ����մϴ�.
    /// </summary>
    public float GetHPMultiplier()
    {
        return 1f + gameTime * hpIncreaseRate;
    }
}
