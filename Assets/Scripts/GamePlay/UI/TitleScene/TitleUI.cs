using UnityEngine;

public class TitleUI : BaseUI
{
    public bool IsAdditionalUIOpened = false;
    

    public void OpenUnlockUI()
    {
        SoundManager.Instance.PlaySFX("ButtonClick", AudioType.UI);
        UIManager.Instance.OpenUI<UnLockUI>(new BaseUIData());
        IsAdditionalUIOpened = true;
    }
    public void OpenInventory()
    {
        SoundManager.Instance.PlaySFX("ButtonClick", AudioType.UI);
        UIManager.Instance.OpenUI<BoxInventoryUI>(new BaseUIData());
        IsAdditionalUIOpened = true;
    }

    public void OpenSettingUI()
    {
        SoundManager.Instance.PlaySFX("ButtonClick", AudioType.UI);
        UIManager.Instance.OpenUI<SettingsUI>(new BaseUIData());
        IsAdditionalUIOpened = true;
    }

    public void OnClickPlayButton()
    {
        SceneLoader.Instance.LoadScene(SceneType.GameScene);
    }
}
