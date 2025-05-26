using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HealthRatioComponent
{
    public float StartTime = 0f;
    public float EndTime = 600f;
    public float startValue = 1f;
    public float endValue = 15f;
    
    private float ratio;            // 현재 비율
    private float timeElapsed;      // 경과 시간


    public void UpdateHealthRatio (float time)
    {
        timeElapsed = time;

        // 경과 시간 비율 t (0 ~ 1)
        float t = Mathf.InverseLerp(StartTime, EndTime, timeElapsed);
        t = Mathf.Clamp01(t); // 보통의 안전장치

        // ease in 효과 적용: t를 비선형으로 (t^2)
        float easeInT = Mathf.Pow(t, 2);

        // 선형 보간에 적용 (t 대신 easeInT)
        ratio = Mathf.Lerp(startValue, endValue, easeInT);

        //Debug.Log($"[Ease In] 시간: {timeElapsed:F1}, 배수: {ratio:F2}");
    }

    public float GetHealthRatio()
    {
        return ratio;
    }
}

