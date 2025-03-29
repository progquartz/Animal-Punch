using UnityEngine;
using System.Collections.Generic;

public class PlayerStabilityChecker : MonoBehaviour
{
    public Transform referenceTransform;

    // �ִ� ��� ���� ��ȭ (Y�ุ üũ)
    public float maxAllowedAngle = 5f;

    // ������ üũ�ϴ� �ð� ����
    public float checkDuration = 1.0f;

    // ���� ����
    public bool boostEnabled { get; private set; }

    private Queue<(float angle, float timestamp)> angleHistory = new Queue<(float, float)>();
    private float minYAngle;
    private float maxYAngle;

    void Start()
    {
        if (referenceTransform == null)
        {
            Debug.LogError("Rigidbody�� Reference Transform�� �������ּ���.");
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

        // ������ ������ ����
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
