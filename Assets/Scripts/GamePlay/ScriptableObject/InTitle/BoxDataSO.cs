using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum BoxRankType
{
    Normal = 0,
    Uncommon = 1,
    Rare = 2,
    Unique = 3,
    Legendary = 4,
    Gold = 5,
    Gem = 6,
}

[CreateAssetMenu(fileName = "BoxData", menuName = "ScriptableObject/Looting/BoxData")]
public class BoxDataSO : ScriptableObject
{
    [Header("루팅 유형 및 등급")]
    public BoxRankType rank;
    [Header("정보")]
    public string displayName;                // 이름
    public Sprite icon;                       // UI 아이콘
    public int unlockDurationSeconds;         // 열리는 데 걸리는 시간
    public float unlockItemPercent;
    [Header("언락 아이템")]
    public List<UnlockableDataSO> unlockableItems; // 포함된 아이템 데이터들.
    public float gemPercent;
    public int gemMin;
    public int gemMax;
    // gem도 안되면 그냥 골드 획득.
    public int goldMin;
    public int goldMax;
    

    [Header("획득 조건 관련 변수")]
    public int minScore;         // 최소 점수
    public int bonusScoreStep;   // 보정 점수 간격
    public float bonusRate;      // 보정 확률 증가량 (%)
    public float baseRate;       // 기본 확률 (%)
    public int dropScore;        // 이 아이템이 차지하는 드랍 점수
}
