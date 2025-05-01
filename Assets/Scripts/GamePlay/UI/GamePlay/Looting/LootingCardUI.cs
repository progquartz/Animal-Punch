using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootingCardUI : MonoBehaviour
{
    private LootingUI owner;
    public TMP_Text CardTitleText;
    public TMP_Text CardLoreText;
    public Image CardBackgroundImage;
    public Image CardImage;

    private ILootEffect lootEffect;

    public void SetupCard(LootingUI owner, LootingTypeType type, LootingRankType rank)
    {
        this.owner = owner;
        lootEffect = LootEffectFactory.CreateEffect(type, rank);
        CardTitleText.text = type.ToString();
        CardLoreText.text = string.Join("\n", lootEffect.GetEffectDescriptions());
        CardBackgroundImage.sprite = LootEffectFactory.GetLootBackgroundImage(rank);
        CardImage.sprite = LootEffectFactory.GetLootTypeImage(type);

        if((int)rank > (int)LootingRankType.Epic)
        {
            ShiningCard();
        }
    }

    private void ShiningCard()
    {

    }

    public void OnClick()
    {
        lootEffect.ApplyEffect(Player.Instance); // Player ½Ì±ÛÅæ »ç¿ë
        owner.CloseLooting(true);
    }
}
