using UnityEngine;

public static class GameTimeEnemyRatio
{
    public static float startTime = 0f;    // 시작 시간
    public static float endTime = 1800f;     // 끝 시간 (30분을 끝으로 가정)

    // 체력 요소
    public static float startHealthMultiplier = 1f;   // 시작 곱하기단위임
    public static float endHealthMultiplier = 3f;     // 끝날때 단위

    // 다른 물리적 / 스탯적 요소 추후에 필요하면 추가.
    

    public static float GetHealthRatio(float elapsedTime)
    {
        elapsedTime += Time.deltaTime;

        float t = Mathf.Clamp01((elapsedTime - startTime) / (endTime - startTime));

        float curveT = Mathf.SmoothStep(0, 1, t); // 더 좋은 함수 뭐 없나

        return Mathf.Lerp(startHealthMultiplier, endHealthMultiplier, curveT);
    }
}
