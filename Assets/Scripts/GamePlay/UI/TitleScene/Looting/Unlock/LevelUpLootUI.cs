using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpLootUI : BaseUI
{
    [System.Serializable]
    public class LevelUpPrize
    {
        public bool isGem;
        public int basePrize;
        public int prizePerFiveStep;
    }
    public Image lootingUI;
    public TMP_Text lootingText;

    public Sprite goldSprite;
    public Sprite gemSprite;

    public List<LevelUpPrize> prizeList;

    public int targetLevel = -1;
    

    public void OpenLoot(int level)
    {
        TitleUI title = UIManager.Instance.GetActiveUI<TitleUI>() as TitleUI;
        if (title != null)
        {
            title.OnAdditionalUIToggled(true);
        }
        targetLevel = level;
        CalculateLoot();


    }

    private void CalculateLoot()
    {
        bool isGem = prizeList[targetLevel % 5].isGem;
        int lootCount = prizeList[targetLevel % 5].basePrize + (prizeList[targetLevel % 5].prizePerFiveStep * (targetLevel / 5));
        if (isGem)
        {
            lootingUI.sprite = gemSprite;
            lootingText.text = lootCount.ToString();
            GameManager.Instance.GetPlayerInfoData().GainGem(lootCount);
        }
        else
        {
            lootingUI.sprite = goldSprite;
            lootingText.text = lootCount.ToString() + " G";
            GameManager.Instance.GetPlayerInfoData().GainGold(lootCount, true);
        }
    }

    public void CloseLoot()
    {
        targetLevel = -1;
        SoundManager.Instance.PlaySFX("ButtonClick", AudioType.UI);
        TitleUI title = UIManager.Instance.GetActiveUI<TitleUI>() as TitleUI;
        if (title != null)
        {
            title.OnAdditionalUIToggled(false);
        }
        Close();
    }
}
