using System;
using UnityEngine;

[System.Serializable]
public class LootingBoxSlot
{
    public BoxDataSO boxData;
    public string startTime;  // 시작 시간 (ISO 8601 형식)

    public bool IsOccupied => boxData != null;


    public bool IsComplete()
    {
        if (!IsOccupied) return false;
        var start = DateTime.Parse(startTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
        return (DateTime.UtcNow - start).TotalSeconds >= boxData.unlockDurationSeconds;
    }

    public float RemainingSeconds()
    {
        var start = DateTime.Parse(startTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
        return Mathf.Max(0, boxData.unlockDurationSeconds - (float)(DateTime.UtcNow - start).TotalSeconds);
    }
}
