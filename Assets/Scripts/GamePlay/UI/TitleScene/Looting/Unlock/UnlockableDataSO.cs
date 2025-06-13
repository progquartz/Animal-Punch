using UnityEngine;

[CreateAssetMenu(fileName = "UnlockableData", menuName = "ScriptableObject/Looting/UnlockableData")]
public class UnlockableDataSO : ScriptableObject
{
    public string id;
    public UnlockCost costType;
    public string displayName;
    public Sprite icon;

    public GameObject modelLow;
    public GameObject modelMedium;
    public GameObject modelHigh;

    public GameObject GetModelByQuality(GraphicsQuality quality)
    {
        return quality switch
        {
            GraphicsQuality.Low => modelLow,
            GraphicsQuality.Medium => modelMedium,
            GraphicsQuality.High => modelHigh,
            _ => modelMedium
        };
    }
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
