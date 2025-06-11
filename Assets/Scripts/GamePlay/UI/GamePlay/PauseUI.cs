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
        GameManager.Instance.EndGameState();
        // 결과 창으로 이동해야 함.
        // 결과 UI가 아래에서 내려오면서... 맵과 관련된 모든걸 다 지우고 새로운 풀링 기법으로 결과창을 보여줘야 함.
        SceneLoader.Instance.LoadScene(SceneType.TitleScene);
        Close();
    }
}
