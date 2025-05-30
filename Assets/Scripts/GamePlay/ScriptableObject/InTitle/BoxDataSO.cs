using UnityEngine;

[CreateAssetMenu(fileName = "BoxData", menuName = "Game/BoxData")]
public class BoxDataSO : ScriptableObject
{
    public string id;                         // 고유 식별자
    public string displayName;                // 이름
    public Sprite icon;                       // UI 아이콘
    public int unlockDurationSeconds;         // 열리는 데 걸리는 시간
}
