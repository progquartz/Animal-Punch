using System;
using UnityEngine;

[System.Serializable]
public class LootingBoxSlot
{
    public string boxId;              // 상자 종류
    public string startTime;         // ISO 8601 형식 UTC
    public int durationSeconds;      // 열리는 데 걸리는 시간 (초)

    public bool IsOccupied => !string.IsNullOrEmpty(boxId);

    public bool IsComplete()
    {
        if (!IsOccupied) return false;
        var start = DateTime.Parse(startTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
        return (DateTime.UtcNow - start).TotalSeconds >= durationSeconds;
    }

    public float RemainingSeconds()
    {
        var start = DateTime.Parse(startTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
        return Mathf.Max(0, durationSeconds - (float)(DateTime.UtcNow - start).TotalSeconds);
    }
}
