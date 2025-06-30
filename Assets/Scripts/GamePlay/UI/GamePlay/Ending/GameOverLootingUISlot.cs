using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverLootingUISlot : MonoBehaviour
{
    public Image LootingUIBackground;
    public Image LootingUIForeground;
    public Image LootingItemImage;
    public TMP_Text LootingCountText;

    public void SetUI(LootingRankDesignTemplate template, int count)
    {
        LootingUIBackground.color = template.backgroundColor;
        LootingUIForeground.color = template.foregroundColor;
        LootingItemImage.sprite = template.sprite;
        LootingCountText.text = count.ToString();
    }
}
