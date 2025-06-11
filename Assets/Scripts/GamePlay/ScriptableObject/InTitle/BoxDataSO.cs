using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoxData", menuName = "ScriptableObject/Looting/BoxData")]
public class BoxDataSO : ScriptableObject
{
    public string id;                         // 고유 식별자
    public string displayName;                // 이름
    public Sprite icon;                       // UI 아이콘
    public int unlockDurationSeconds;         // 열리는 데 걸리는 시간
    public List<UnlockableDataSO> unlockableItems; // 포함된 아이템 데이터들.
}
