using UnityEngine;

public class PauseUI : BaseUI
{
    public void OnClickResumeButton()
    {
        GameManager.Instance.ResumeTime();
        Close();
    }

    public void OnClickSettingButton()
    {
        UIManager.Instance.OpenUI<SettingsUI>(new BaseUIData());
    }

    public void OnClickQuitButton()
    {
        GameManager.Instance.OnClickQuitGameOver();
        Close();
    }
}
