using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettingUI : MonoBehaviour
{
    public Toggle lowToggle;
    public Toggle mediumToggle;
    public Toggle highToggle;

    public Toggle aaOffToggle;
    public Toggle aaMediumToggle;
    public Toggle aaHighToggle;

    private void Start()
    {
        ApplySavedState();

        lowToggle.onValueChanged.AddListener((v) => { if (v) SetQuality(GraphicsQuality.Low); });
        mediumToggle.onValueChanged.AddListener((v) => { if (v) SetQuality(GraphicsQuality.Medium); });
        highToggle.onValueChanged.AddListener((v) => { if (v) SetQuality(GraphicsQuality.High); });

        aaOffToggle.onValueChanged.AddListener((v) => { if (v) SetAA(AntiAliasingLevel.Off); });
        aaMediumToggle.onValueChanged.AddListener((v) => { if (v) SetAA(AntiAliasingLevel.Medium); });
        aaHighToggle.onValueChanged.AddListener((v) => { if (v) SetAA(AntiAliasingLevel.High); });
    }

    void SetQuality(GraphicsQuality quality)
    {
        SettingsManager.Instance.graphicsQuality = quality;
        SettingsManager.Instance.SaveSettings();
    }

    void SetAA(AntiAliasingLevel level)
    {
        SettingsManager.Instance.antiAliasing = level;
        SettingsManager.Instance.SaveSettings();
    }

    void ApplySavedState()
    {
        switch (SettingsManager.Instance.graphicsQuality)
        {
            case GraphicsQuality.Low: lowToggle.isOn = true; break;
            case GraphicsQuality.Medium: mediumToggle.isOn = true; break;
            case GraphicsQuality.High: highToggle.isOn = true; break;
        }

        switch (SettingsManager.Instance.antiAliasing)
        {
            case AntiAliasingLevel.Off: aaOffToggle.isOn = true; break;
            case AntiAliasingLevel.Medium: aaMediumToggle.isOn = true; break;
            case AntiAliasingLevel.High: aaHighToggle.isOn = true; break;
        }
    }
}
