using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : BaseUI
{
    public ScrollRect scrollRect;

    public override void Init(Transform canvas)
    {
        transform.SetParent(canvas);

        var rectTransform = transform as RectTransform;
        rectTransform.localPosition = Vector3.zero;
        rectTransform.localScale = Vector3.one;
        rectTransform.localRotation = Quaternion.identity;
        RearrangePosition();
    }

    private void RearrangePosition()
    {
        scrollRect.horizontalNormalizedPosition = 0f;
    }

    public override void Close(bool isCloseAll = false)
    {
        TitleUI title = UIManager.Instance.GetActiveUI<TitleUI>() as TitleUI;
        if(title != null)
        {
            title.IsAdditionalUIOpened = false;
        }
        base.Close(isCloseAll);
    }

}
