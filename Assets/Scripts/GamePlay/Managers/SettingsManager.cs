using UnityEngine;

public enum GraphicsQuality { Low, Medium, High }
public enum AntiAliasingLevel { Off, Medium, High }
public enum FrameLimit { Fps30, Fps60, Unlimited }

[System.Serializable]
public class VolumeSettings
{
    public float volume;
    public bool muted;
}

public class SettingsManager : SingletonBehaviour<SettingsManager>
{

    public GraphicsQuality graphicsQuality;
    public AntiAliasingLevel antiAliasing;
    public FrameLimit frameLimit;

    public VolumeSettings masterVolume = new VolumeSettings();
    public VolumeSettings bgmVolume = new VolumeSettings();
    public VolumeSettings playerVolume = new VolumeSettings();
    public VolumeSettings enemyVolume = new VolumeSettings();

    protected override void Init()
    {
        base.Init();
        LoadSettings();
    }

    // 사용 하는 법.
    void ApplyGraphicsSettings()
    {
        var quality = SettingsManager.Instance.graphicsQuality;
        QualitySettings.SetQualityLevel((int)quality);

        var aa = SettingsManager.Instance.antiAliasing;
        QualitySettings.antiAliasing = aa switch
        {
            AntiAliasingLevel.Off => 0,
            AntiAliasingLevel.Medium => 2,
            AntiAliasingLevel.High => 4,
            _ => 0
        };
    }


    public void SaveSettings()
    {
        PlayerPrefs.SetInt("GraphicsQuality", (int)graphicsQuality);
        PlayerPrefs.SetInt("AntiAliasing", (int)antiAliasing);
        PlayerPrefs.SetInt("FrameLimit", (int)frameLimit);

        SaveVolume("Master", masterVolume);
        SaveVolume("BGM", bgmVolume);
        SaveVolume("Player", playerVolume);
        SaveVolume("Enemy", enemyVolume);

        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        graphicsQuality = (GraphicsQuality)PlayerPrefs.GetInt("GraphicsQuality", 1);
        antiAliasing = (AntiAliasingLevel)PlayerPrefs.GetInt("AntiAliasing", 1);
        frameLimit = (FrameLimit)PlayerPrefs.GetInt("FrameLimit", 1);

        masterVolume = LoadVolume("Master");
        bgmVolume = LoadVolume("BGM");
        playerVolume = LoadVolume("Player");
        enemyVolume = LoadVolume("Enemy");
    }

    private void SaveVolume(string key, VolumeSettings setting)
    {
        PlayerPrefs.SetFloat($"{key}Volume", setting.volume);
        PlayerPrefs.SetInt($"{key}Muted", setting.muted ? 1 : 0);
    }

    private VolumeSettings LoadVolume(string key)
    {
        return new VolumeSettings
        {
            volume = PlayerPrefs.GetFloat($"{key}Volume", 1f),
            muted = PlayerPrefs.GetInt($"{key}Muted", 0) == 1
        };
    }
}
