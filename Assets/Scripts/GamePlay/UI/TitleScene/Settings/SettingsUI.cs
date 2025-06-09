using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : BaseUI
{
    public ScrollRect scrollRect;
    public override void Show()
    {
        base.Show();

    }
    private void RearrangePosition()
    {
        scrollRect.horizontalNormalizedPosition = 0f;
    }

    public override void Close(bool isCloseAll = false)
    {
        base.Close(isCloseAll);

    }

}
