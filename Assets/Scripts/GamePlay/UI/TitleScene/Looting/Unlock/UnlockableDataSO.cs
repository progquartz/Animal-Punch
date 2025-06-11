using UnityEngine;

[CreateAssetMenu(fileName = "UnlockableData", menuName = "ScriptableObject/Looting/UnlockableData")]
public class UnlockableDataSO : ScriptableObject
{
    public string id;
    public UnlockCost costType;
    public string displayName;
    public Sprite icon;
    public GameObject UnlockPrefab;
    // 캐릭터 프리팹 연결 방법 고민해보기....
}

[System.Serializable]
public class UnlockCost
{
    public CostType costType;
    public int cost;
    
}

public enum CostType
{
    Gold,
    Gem
}
