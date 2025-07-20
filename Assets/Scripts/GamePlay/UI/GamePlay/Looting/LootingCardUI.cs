using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LootingCardUI : MonoBehaviour
{
    [System.Serializable]
    public class LootingCardImagePalette
    {
        public Color backgroundFrame;
        public Color background;
        public Color loreground;
    }

    private LootingUI owner;
    public TMP_Text CardTitleText;
    public TMP_Text CardLoreText;
    public Image CardBackgroundImage;
    public Image CardBackgroundFrameImage;
    public Image CardLoregroundImage;
    public Image CardImage;
    public List<LootingCardImagePalette> cardPaletteList;
    

    private ILootEffect lootEffect;

    public void SetupCard(LootingUI owner, LootingTypeType type, LootingRankType rank)
    {
        this.owner = owner;
        lootEffect = LootEffectFactory.CreateEffect(type, rank);
        if(lootEffect == null)
        {
            Debug.LogError($"{type.ToString()} 을 만들었는데, 그 결과가 null값으로 도출됩니다.");
        }

        CardTitleText.text = type.ToString();
        CardLoreText.text = string.Join("\n", lootEffect.GetEffectDescriptions());

        CardBackgroundImage.color = cardPaletteList[(int)rank].background;
        CardBackgroundFrameImage.color = cardPaletteList[(int)rank].backgroundFrame;
        CardLoregroundImage.color = cardPaletteList[(int)rank].loreground;
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
        SoundManager.Instance.PlaySFX("LootingUISelect", AudioType.UI);
        lootEffect.ApplyEffect(Player.Instance);
        owner.CloseLooting(true);
    }
}
