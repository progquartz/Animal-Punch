using UnityEngine;

[System.Serializable]
public class LootingRankDesignTemplate
{
    public LootingRankType rank;
    public Color backgroundColor;
    public Color foregroundColor;
    public Sprite Sprite;
}

public class GameOverLootingUI : MonoBehaviour
{
    public Transform ViewportTransform;
    public LootingRankDesignTemplate[] rankDesignTemplateList;

    public void ShowLoot()
    {
        
    }


}
