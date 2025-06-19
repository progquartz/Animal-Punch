using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HealthRatioComponent
{
    [Header("Multiplier Settings")]
    private float maxRatio = 20f;
    private float maxTime = 1800f;
    [Range(1f, 5f)] private float easePower = 1f; // 높을수록 천천히 시작해서 점점 가속

    private float currentTime = 0f;
    public float Ratio { get; private set; } = 1f;

    public void UpdateRatio()
    {
        currentTime = GameManager.Instance.GameTime;
        currentTime = Mathf.Min(currentTime, maxTime); // maxtime 이상 증가하지 않도록 만들기.

        float normalizedTime = currentTime / maxTime;
        Debug.Log(maxTime + " " + maxRatio + " " +   normalizedTime + " " + Ratio);
        Ratio = Mathf.Lerp(1f, maxRatio, normalizedTime); // 비율 보간
    }
}

