using UnityEngine;

public class StaticUIs : MonoBehaviour
{
    public void OnClickPauseButtonUI()
    {
        UIManager.Instance.OpenUI<PauseUI>(new BaseUIData());
        SoundManager.Instance.PlaySFX("ButtonClick");
        GameManager.Instance.StopTime();
    }
}
