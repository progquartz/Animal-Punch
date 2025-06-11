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
            "Player" => settings.playerVolume,
            "Enemy" => settings.enemyVolume,
            _ => new VolumeSettings()
        };

        volumeSlider.value = currentVolume.volume;
        muteToggle.isOn = currentVolume.muted;
        ApplyVolume();
    }

    void OnVolumeChanged(float value)
    {
        currentVolume.volume = value;
        SaveVolume();
        ApplyVolume();
    }

    void OnMuteToggled(bool isMuted)
    {
        currentVolume.muted = isMuted;
        SaveVolume();
        ApplyVolume();
    }

    void SaveVolume()
    {
        switch (volumeKey)
        {
            case "Master": SettingsManager.Instance.masterVolume = currentVolume; break;
            case "BGM": SettingsManager.Instance.bgmVolume = currentVolume; break;
            case "Player": SettingsManager.Instance.playerVolume = currentVolume; break;
            case "Enemy": SettingsManager.Instance.enemyVolume = currentVolume; break;
        }

        SettingsManager.Instance.SaveSettings();
    }

    void ApplyVolume()
    {
        float volumeToApply = currentVolume.muted ? 0f : currentVolume.volume;

        // 예시 적용: AudioMixer 사용 시 아래 라인처럼 적용
        // AudioMixer.SetFloat($"{volumeKey}Volume", Mathf.Log10(volumeToApply) * 20);

        Debug.Log($"{volumeKey} 볼륨 적용됨: {volumeToApply}");
    }
}
