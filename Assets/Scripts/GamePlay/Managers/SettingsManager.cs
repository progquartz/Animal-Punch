using System;
using UnityEngine;

public enum GraphicsQuality { Low, Medium, High }
public enum AntiAliasingLevel { Off, Medium, High }
public enum FrameLimit { Fps30, Fps60, Unlimited }
public enum AudioType { Master, Bgm, Entity, UI }

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
    public VolumeSettings entityVolume = new VolumeSettings();
    public VolumeSettings uiVolume = new VolumeSettings();

    public Action<VolumeSettings> masterVolumeChanged;
    public Action<VolumeSettings> bgmVolumeChanged;
    public Action<VolumeSettings> entityVolumeChanged;
    public Action<VolumeSettings> uiVolumeChanged;

    public Action<GraphicsQuality> graphicsQualityChanged;
    public Action<FrameLimit> frameLimitChanged;

    protected override void Init()
    {
        base.Init();
        LoadSettings();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("GraphicsQuality", (int)graphicsQuality);
        PlayerPrefs.SetInt("AntiAliasing", (int)antiAliasing);
        PlayerPrefs.SetInt("FrameLimit", (int)frameLimit);

        SaveVolume("Master", masterVolume);
        SaveVolume("BGM", bgmVolume);
        SaveVolume("Entity", entityVolume);
        SaveVolume("UI", uiVolume);

        graphicsQualityChanged?.Invoke(graphicsQuality);
        frameLimitChanged?.Invoke(frameLimit);
        masterVolumeChanged?.Invoke(masterVolume);
        bgmVolumeChanged?.Invoke(bgmVolume);
        entityVolumeChanged?.Invoke(entityVolume);
        uiVolumeChanged?.Invoke(uiVolume);

        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        graphicsQuality = (GraphicsQuality)PlayerPrefs.GetInt("GraphicsQuality", 1);
        antiAliasing = (AntiAliasingLevel)PlayerPrefs.GetInt("AntiAliasing", 1);
        frameLimit = (FrameLimit)PlayerPrefs.GetInt("FrameLimit", 1);

        masterVolume = LoadVolume("Master");
        bgmVolume = LoadVolume("BGM");
        entityVolume = LoadVolume("Entity");
        uiVolume = LoadVolume("UI");
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
