using TMPro;
using UnityEngine;

public class TitleBackgroundUI : BaseUI
{
    public TextMeshProUGUI tmpText;
    public float scaleMin = 1f;
    public float scaleMax = 1.15f;
    public float duration = 4f; 

    void Update()
    {
        float t = Mathf.PingPong(Time.time / (duration / 2f), 1f); 
        float scale = Mathf.Lerp(scaleMin, scaleMax, t);
        tmpText.rectTransform.localScale = Vector3.one *  scale;
    }

    public void OnClickButton()
    {
        TitleUI title = UIManager.Instance.GetActiveUI<TitleUI>() as TitleUI;
        title.OnAdditionalUIToggled(false);
        Close();
    }
}
