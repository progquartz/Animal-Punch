using UnityEngine;

public class VideoSettingComponent
{
    public void SetFrameRate(FrameLimit option)
    {
        QualitySettings.vSyncCount = 0; // VSync ²û
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
