using UnityEngine;

public class TitleUI : BaseUI
{
    public bool IsAdditionalUIOpened = false;
    

    public void OpenUnlockUI()
    {
        UIManager.Instance.OpenUI<UnLockUI>(new BaseUIData());
        IsAdditionalUIOpened = true;
    }
    public void OpenInventory()
    {
        UIManager.Instance.OpenUI<BoxInventoryUI>(new BaseUIData());
        IsAdditionalUIOpened = true;
    }

    public void OpenSettingUI()
    {
        UIManager.Instance.OpenUI<SettingsUI>(new BaseUIData());
        IsAdditionalUIOpened = true;
    }

    public void OnClickPlayButton()
    {
        SceneLoader.Instance.LoadScene(SceneType.GameScene);
    }
}
