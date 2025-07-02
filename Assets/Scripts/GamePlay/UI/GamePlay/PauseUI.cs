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
        // 설정창 열리기.
        // UIManager.Instance.OpenUI<BoxInventoryUI>(new BaseUIData());
        UIManager.Instance.OpenUI<SettingsUI>(new BaseUIData());
    }

    public void OnClickQuitButton()
    {
        SceneLoader.Instance.LoadScene(SceneType.TitleScene);
        Close();
    }
}
