using UnityEngine;
using UnityEngine.UI;

public class TitleAndroidBackHandler : MonoBehaviour
{
    [SerializeField] private GameObject quitPopupUI;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private void Start()
    {
        quitPopupUI.SetActive(false);

        yesButton.onClick.AddListener(OnQuitConfirmed);
        noButton.onClick.AddListener(OnCancelQuit);
    }

    private void Update()
    {
        if (ShouldTriggerBack() && !IsUITurnedOn())
        {
            if (!quitPopupUI.activeSelf)
            {
                SoundManager.Instance.PlaySFX("ButtonClick", AudioType.UI);
                quitPopupUI.SetActive(true);
            }
            else
            {
                SoundManager.Instance.PlaySFX("ButtonClick", AudioType.UI);
                quitPopupUI.SetActive(false);
            }
        }
    }

    private bool IsUITurnedOn()
    {
        TitleUI titleUI = UIManager.Instance.GetActiveUI<TitleUI>() as TitleUI;
        return titleUI.IsAdditionalUIOpened;
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

    private void OnQuitConfirmed()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#endif
    }

    private void OnCancelQuit()
    {
        quitPopupUI.SetActive(false);
    }
}
