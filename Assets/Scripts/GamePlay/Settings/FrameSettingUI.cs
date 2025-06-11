using UnityEngine;
using UnityEngine.UI;

public class FrameSettingUI : MonoBehaviour
{
    public Toggle fps30Toggle;
    public Toggle fps60Toggle;
    public Toggle unlimitedToggle;

    void Start()
    {
        ApplySavedState();

        fps30Toggle.onValueChanged.AddListener((v) => 
        {
            if (v) SetFrame(FrameLimit.Fps30); 
        });

        fps60Toggle.onValueChanged.AddListener((v) => 
        {
            if (v) SetFrame(FrameLimit.Fps60); 
        });

        unlimitedToggle.onValueChanged.AddListener((v) => 
        {
            if (v) SetFrame(FrameLimit.Unlimited); 
        });

    }

    void SetFrame(FrameLimit limit)
    {
        SettingsManager.Instance.frameLimit = limit;
        SettingsManager.Instance.SaveSettings();
        ApplyFrameSetting();
    }

    void ApplySavedState()
    {
        switch (SettingsManager.Instance.frameLimit)
        {
            case FrameLimit.Fps30: fps30Toggle.isOn = true; break;
            case FrameLimit.Fps60: fps60Toggle.isOn = true; break;
            case FrameLimit.Unlimited: unlimitedToggle.isOn = true; break;
        }

        ApplyFrameSetting();
    }

    void ApplyFrameSetting()
    {
        Application.targetFrameRate = SettingsManager.Instance.frameLimit switch
        {
            FrameLimit.Fps30 => 30,
            FrameLimit.Fps60 => 60,
            FrameLimit.Unlimited => -1,
            _ => 60
        };
    }
}
