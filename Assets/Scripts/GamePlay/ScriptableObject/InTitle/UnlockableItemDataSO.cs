using UnityEngine;

[CreateAssetMenu(fileName = "UnlockableItemData", menuName = "Game/UnlockableItem")]
public class UnlockableItemDataSO : ScriptableObject
{
    public string id;              // 고유 ID
    public string displayName;     // 이름
    public Sprite icon;            // UI에 띄울 아이콘
    public int cost;               // 언락 비용
    public GameObject prefab;      // 실제 사용할 프리팹
}
