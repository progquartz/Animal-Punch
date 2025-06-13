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
            "Player" => settings.entityVolume,
            "Enemy" => settings.uiVolume,
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
            case "Player": SettingsManager.Instance.entityVolume = currentVolume; break;
            case "Enemy": SettingsManager.Instance.uiVolume = currentVolume; break;
        }

        SettingsManager.Instance.SaveSettings();
    }

    void ApplyVolume()
    {
        float volumeToApply = currentVolume.muted ? 0f : currentVolume.volume;
        Debug.Log($"{volumeKey} º¼·ý Àû¿ëµÊ: {volumeToApply}");
    }
}
