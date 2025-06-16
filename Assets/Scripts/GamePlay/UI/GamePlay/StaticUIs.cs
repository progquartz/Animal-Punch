using UnityEngine;

public class StaticUIs : MonoBehaviour
{
    public void OnClickPauseButtonUI()
    {
        UIManager.Instance.OpenUI<PauseUI>(new BaseUIData());
        SoundManager.Instance.PlaySFX("ButtonClick", AudioType.UI);
        GameManager.Instance.StopTime();
    }

    public void OnClickSettingButtonUI()
    {
        UIManager.Instance.OpenUI<SettingsUI>(new BaseUIData());    

    }
}
