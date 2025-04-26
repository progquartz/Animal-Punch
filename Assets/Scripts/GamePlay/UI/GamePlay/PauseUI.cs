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
    }

    public void OnClickQuitButton()
    {
        Close();
    }
}
