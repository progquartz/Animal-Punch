using UnityEngine;
using UnityEngine.UI;

public class StaticUIs : MonoBehaviour
{
    [SerializeField]
    private PauseUI openedPauseUI;

    private void Update()
    {
        if (ShouldTriggerBack())
        {
            if (!GameManager.Instance.IsGamePaused)
            {
                OnClickPauseButtonUI();
            }
            else
            {
                if(openedPauseUI != null)
                {
                    openedPauseUI.OnClickResumeButton();
                    openedPauseUI = null;
                }
            }
        }
    }

    private bool ShouldTriggerBack()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            return Input.GetKeyDown(KeyCode.Escape);
        }

        if (Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.OSXPlayer ||
            Application.platform == RuntimePlatform.LinuxPlayer ||
            Application.isEditor) // 에디터 테스트용
        {
            return Input.GetKeyDown(KeyCode.Escape);
        }

        return false;
    }

    public void OnClickPauseButtonUI()
    {
        UIManager.Instance.OpenUI<PauseUI>(new BaseUIData());
        openedPauseUI = UIManager.Instance.GetActiveUI<PauseUI>() as PauseUI;
        SoundManager.Instance.PlaySFX("ButtonClick", AudioType.UI);
        GameManager.Instance.StopTime();
    }

    public void OnClickSettingButtonUI()
    {
        UIManager.Instance.OpenUI<SettingsUI>(new BaseUIData());    
    }


}
