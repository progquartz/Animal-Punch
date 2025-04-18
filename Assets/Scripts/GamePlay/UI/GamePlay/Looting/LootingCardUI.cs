using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootingCardUI : MonoBehaviour
{
    public TMP_Text CardTitleText;
    public TMP_Text CardLoreText;
    public Image CardImage;

    private ILootEffect lootEffect;

    public void SetupCard(LootingTypeType type, LootingRankType rank, Color rankColor)
    {
        lootEffect = LootEffectFactory.CreateEffect(type, rank);
        CardTitleText.text = type.ToString();
        CardLoreText.text = string.Join("\n", lootEffect.GetEffectDescriptions());
        CardImage.color = rankColor;
    }

    public void OnClick()
    {
        lootEffect.ApplyEffect(Player.Instance); // Player 싱글톤 사용
        gameObject.SetActive(false); // 카드 비활성화 or UI 종료 등
    }
}
