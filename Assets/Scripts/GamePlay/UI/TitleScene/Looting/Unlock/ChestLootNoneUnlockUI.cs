using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestLootNoneUnlockUI : BaseUI
{
    public Image lootingUI;
    public TMP_Text lootingText;

    public Sprite goldSprite;
    public Sprite gemSprite;

    public void OpenLoot(bool isGem, int amouunt)
    {
        TitleUI title = UIManager.Instance.GetActiveUI<TitleUI>() as TitleUI;
        if (title != null)
        {
            title.OnAdditionalUIToggled(true);
        }

        if (isGem)
        {
            lootingUI.sprite = gemSprite;
            lootingText.text = amouunt.ToString();
        }
        else
        {
            lootingUI.sprite = goldSprite;
            lootingText.text = amouunt.ToString() + " G";
        }
    }

    public void CloseLoot()
    {
        TitleUI title = UIManager.Instance.GetActiveUI<TitleUI>() as TitleUI;
        if (title != null)
        {
            title.OnAdditionalUIToggled(false);
        }
        Close();
    }
}
