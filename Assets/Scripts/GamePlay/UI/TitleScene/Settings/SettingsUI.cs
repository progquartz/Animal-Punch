using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : BaseUI
{
    public ScrollRect scrollRect;
    private bool initialized = false;


    public override void Init(Transform canvas)
    {
        transform.SetParent(canvas);
        var rectTransform = transform as RectTransform;
        rectTransform.localPosition = Vector3.zero;
        rectTransform.localScale = Vector3.one;
        rectTransform.localRotation = Quaternion.identity;
        RearrangePosition();

        StartCoroutine(EnableInteraction());
    }

    IEnumerator EnableInteraction()
    {
        yield return null; 
        initialized = true;
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
            title.OnAdditionalUIToggled(false);
        }
        SoundManager.Instance.PlaySFX("ButtonClick", AudioType.UI);
        base.Close(isCloseAll);
    }

    public void OnValueChangeSFX()
    {
        if (!initialized) return;
        SoundManager.Instance.PlaySFX("ButtonClick", AudioType.UI);
    }

}
