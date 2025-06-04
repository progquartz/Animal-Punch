using UnityEngine;

public class TitleUI : BaseUI
{

    public void OpenInventory()
    {
        UIManager.Instance.OpenUI<BoxInventoryUI>(new BaseUIData());
    }
}
