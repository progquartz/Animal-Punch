using UnityEngine;

public class VideoSettingComponent
{
    public void SetFrameRate(FrameLimit option)
    {

        if (GameManager.Instance.IsGamePaused)
        {
            Debug.Log("시간 멈춤 중에는 프레임 설정을 적용하지 않음");
            return;
        }


        QualitySettings.vSyncCount = 0; // VSync 끔
        switch (option)
        {
            case FrameLimit.Fps30:
                Application.targetFrameRate = 30;
                break;
            case FrameLimit.Fps60:
                Application.targetFrameRate = 60;
                break;
            case FrameLimit.Unlimited:
                Application.targetFrameRate = -1;
                break;
        }
    }
}
