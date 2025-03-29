using UnityEngine;
using System.Collections.Generic;

public class PlayerStabilityChecker : MonoBehaviour
{
    public Transform referenceTransform;

    // 최대 허용 각도 변화 (Y축만 체크)
    public float maxAllowedAngle = 5f;

    // 각도를 체크하는 시간 간격
    public float checkDuration = 1.0f;

    // 상태 변수
    public bool boostEnabled { get; private set; }

    private Queue<(float angle, float timestamp)> angleHistory = new Queue<(float, float)>();
    private float minYAngle;
    private float maxYAngle;

    void Start()
    {
        if (referenceTransform == null)
        {
            Debug.LogError("Rigidbody와 Reference Transform을 설정해주세요.");
            enabled = false;
            return;
        }

        ResetStability();
    }

    public bool CheckBoostEnabled(Transform reference)
    {
        if(referenceTransform == null)
        {
            referenceTransform = transform;
        }
        float currentYAngle = NormalizeAngle(referenceTransform.eulerAngles.y);
        float currentTime = Time.time;

        angleHistory.Enqueue((currentYAngle, currentTime));

        // 오래된 데이터 제거
        while (angleHistory.Count > 0 && currentTime - angleHistory.Peek().timestamp > checkDuration)
            angleHistory.Dequeue();

        UpdateMinMaxAngles();

        boostEnabled = (maxYAngle - minYAngle) <= maxAllowedAngle;
        return boostEnabled;
        //Debug.Log(boostEnabled);
    }

    private void UpdateMinMaxAngles()
    {
        minYAngle = float.MaxValue;
        maxYAngle = float.MinValue;

        foreach (var record in angleHistory)
        {
            minYAngle = Mathf.Min(minYAngle, record.angle);
            maxYAngle = Mathf.Max(maxYAngle, record.angle);
        }
    }

    private float NormalizeAngle(float angle)
    {
        if (angle > 180f)
            angle -= 360f;
        return angle;
    }

    public void ResetStability()
    {
        angleHistory.Clear();
        boostEnabled = false;
    }
}
