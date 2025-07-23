using UnityEngine;
using UnityEngine.UI;

public class VolumeSettingUI : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle muteToggle;
    public string volumeKey;

    private VolumeSettings currentVolume;

    void Start()
    {
        LoadVolumeSetting();
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        muteToggle.onValueChanged.AddListener(OnMuteToggled);
    }

    void LoadVolumeSetting()
    {
        var settings = SettingsManager.Instance;
        currentVolume = volumeKey switch
        {
            "Master" => settings.masterVolume,
            "BGM" => settings.bgmVolume,
            "Entity" => settings.entityVolume,
            "UI" => settings.uiVolume,
            "SFX" => settings.sfxVolume,
            _ => new VolumeSettings()
        };

        volumeSlider.value = currentVolume.volume;
        muteToggle.isOn = currentVolume.muted;
        ApplyVolume();
    }

    void OnVolumeChanged(float value)
    {
        currentVolume.volume = value;
        ApplyVolume();
        SaveVolume();
    }

    void OnMuteToggled(bool isMuted)
    {
        currentVolume.muted = isMuted;
        ApplyVolume();
        SaveVolume();
    }

    void SaveVolume()
    {
        switch (volumeKey)
        {
            case "Master": SettingsManager.Instance.masterVolume = currentVolume; break;
            case "BGM": SettingsManager.Instance.bgmVolume = currentVolume; break;
            case "Entity": SettingsManager.Instance.entityVolume = currentVolume; break;
            case "UI": SettingsManager.Instance.uiVolume = currentVolume; break;
            case "SFX":SettingsManager.Instance.sfxVolume = currentVolume; break;
        }

        SettingsManager.Instance.SaveSettings();
        SoundManager.Instance.ApplyVolumeFromSettings();
    }

    void ApplyVolume()
    {
        float volumeToApply = currentVolume.muted ? 0f : currentVolume.volume;
        Debug.Log($"{volumeKey} 볼륨 적용됨: {volumeToApply}");
    }
}
