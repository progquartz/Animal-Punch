using UnityEngine;

public class StaticUIs : MonoBehaviour
{
    public void OnClickPauseButtonUI()
    {
        UIManager.Instance.OpenUI<PauseUI>(new BaseUIData());
        GameManager.Instance.StopTime();
    }
}
