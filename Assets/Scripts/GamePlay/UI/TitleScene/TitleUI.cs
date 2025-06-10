using UnityEngine;

public class TitleUI : BaseUI
{
    public void OpenUnlockUI()
    {
        UIManager.Instance.OpenUI<UnLockUI>(new BaseUIData());
    }
    public void OpenInventory()
    {
        UIManager.Instance.OpenUI<BoxInventoryUI>(new BaseUIData());
    }

    public void OpenSettingUI()
    {
        UIManager.Instance.OpenUI<SettingsUI>(new BaseUIData());
    }
}
