using UnityEngine;

public class TitleBackgroundUI : BaseUI
{
    public void OnClickButton()
    {
        TitleUI title = UIManager.Instance.GetActiveUI<TitleUI>() as TitleUI;
        title.IsAdditionalUIOpened = false;
        Close();
    }
}
