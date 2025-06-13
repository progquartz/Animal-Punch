using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using static Unity.VisualScripting.Member;
using System.Runtime.CompilerServices;

public class SoundManager : SingletonBehaviour<SoundManager>
{
    public Transform SoundParent;
    public int GlobalVolume;
    [SerializeField] private SoundDataStorage soundDataStorage;
    [SerializeField] private int poolSize = 10;

    private Dictionary<string, AudioClipData> soundClips = new();
    private Queue<AudioSource> sfxPool = new();
    private List<AudioSource> inUse = new(); // 사용중인 경우

    // 볼륨 제어 부분
    public AudioMixerGroup masterAudioGroup;
    public AudioMixerGroup sfxEntityGroup;
    public AudioMixerGroup sfxUIGroup;
    public AudioMixerGroup bgmGroup;
    public AudioMixer audioMixer;

    private AudioSource bgmSource;
    private bool isPaused = false;

    private string masterVolumeKey = "MasterVolume";
    private string bgmVolumeKey = "BGMVolume";
    private string sfxEntityVolumeKey = "SFXEntityVolume";
    private string sfxUIVolumeKey = "SFXUIVolume";

    protected override void Init()
    {
        base.Init();
        InitSounds();
        InitSFXPool();
        SceneManager.sceneLoaded += OnSceneLoaded;
        RegisterEvent();
        PlayBGM("MainBGM");
        SoundParent = GameObject.Find("SoundParent").transform;
        
    }

    private void RegisterEvent()
    {
        SettingsManager.Instance.masterVolumeChanged += SetMasterVolume;
        SettingsManager.Instance.bgmVolumeChanged += SetBGMVolume;
        SettingsManager.Instance.entityVolumeChanged += SetSFXEntityVolume;
        SettingsManager.Instance.uiVolumeChanged += SetSFXUIVolume;
    }

    private void InitSounds()
    {
        foreach (var entry in soundDataStorage.sounds)
        {
            if (!soundClips.ContainsKey(entry.key))
                soundClips.Add(entry.key, new AudioClipData(entry.clip, entry.volume, entry.pitch));
        }

        foreach(var entry in DataManager.Instance.EnemyDataStorage._enemyData)
        {
            if (!soundClips.ContainsKey(entry.ActorKey + "Dead") && entry.DeadClip.clip.Count > 0)
            {
                soundClips.Add((entry.ActorKey + "Dead"), new AudioClipData(entry.DeadClip.clip, entry.DeadClip.volume, entry.DeadClip.pitch));
            }
        }
    }

    private void InitSFXPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var source = new GameObject("SFXSource").AddComponent<AudioSource>();
            source.transform.SetParent(SoundParent);
            source.playOnAwake = false;
            sfxPool.Enqueue(source);
        }
    }

    /// <summary>
    /// 사운드 재생, 단 캐릭터에게서 나는 소리일 경우에는 pos를 지정.
    /// UI 효과음 등은, 효과음을 지정하지 않아도 됨.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="pos"></param>
    public void PlaySFX(string key , AudioType audioType, Vector3? pos = null)
    {
        if (!soundClips.TryGetValue(key, out var data)) return;

        if (sfxPool.Count == 0) return;

        var sfxSource = sfxPool.Dequeue();
        inUse.Add(sfxSource);

        if (pos.HasValue)
        {
            sfxSource.transform.position = pos.Value;
            sfxSource.spatialBlend = 1f; // 공간 음향 적용
        }
        else
        {
            sfxSource.spatialBlend = 0f; // 공간 음향 적용 안함.
        }

        switch(audioType)
        {
            case AudioType.Entity:
                sfxSource.outputAudioMixerGroup = sfxEntityGroup; // 볼륨 그룹 나누기
                break;
            case AudioType.Bgm:
                sfxSource.outputAudioMixerGroup = bgmGroup; // 볼륨 그룹 나누기
                break;
            case AudioType.UI:
                sfxSource.outputAudioMixerGroup = sfxUIGroup;
                break;
            default:
                sfxSource.outputAudioMixerGroup = sfxEntityGroup;
                break;
        }

        sfxSource.volume = data.volume;
        sfxSource.clip = data.GetRandomAudioClip();
        sfxSource.pitch = data.pitch;
        sfxSource.Play();

        StartCoroutine(ReturnToPoolWhenDone(sfxSource));
    }

    /// <summary>
    /// BGM 재생
    /// </summary>
    /// <param name="key"></param>
    /// <param name="loop"></param>
    public void PlayBGM(string key, bool loop = true)
    {
        if (!soundClips.TryGetValue(key, out var data)) return;

        if (bgmSource == null)
        {
            bgmSource = new GameObject("BGMSource").AddComponent<AudioSource>();
            bgmSource.loop = loop;
        }

        bgmSource.outputAudioMixerGroup = bgmGroup; // 볼륨 그룹 나누기
        bgmSource.clip = data.GetRandomAudioClip();
        bgmSource.volume = data.volume;
        bgmSource.pitch = data.pitch;
        bgmSource.Play();
    }

    private IEnumerator ReturnToPoolWhenDone(AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);
        source.clip = null;
        inUse.Remove(source);
        sfxPool.Enqueue(source);
    }



    public void StopBGM() => bgmSource?.Stop();

    // 씬이 로드될 때 모든 SFX를 멈추기
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopAllSFX();
        SoundParent = GameObject.Find("SoundParent").transform;
    }

    // 모든 SFX를 멈추고 풀로 반환
    private void StopAllSFX()
    {
        foreach (var source in inUse)
        {
            source.Stop();
            sfxPool.Enqueue(source); // 풀에 다시 넣기
        }
        inUse.Clear();
    }

    // 게임 일시 정지 상태 변경
    public void SetPauseState(bool paused)
    {
        isPaused = paused;
        if (isPaused)
        {
            StopAllSFX();  // SFX 멈추기
        }
    }

    public void SetMasterVolume(VolumeSettings value)
    {
        if(value.muted)
        {
            audioMixer.SetFloat(masterVolumeKey, CalculateSliderVolume(0f));
        }
        else
        {
            audioMixer.SetFloat(masterVolumeKey, CalculateSliderVolume(value.volume));
        }
        
    }


    public void SetBGMVolume(VolumeSettings value)
    {
        if(value.muted)
        {
            audioMixer.SetFloat(bgmVolumeKey, CalculateSliderVolume(0f));
        }
        else
        {
            audioMixer.SetFloat(bgmVolumeKey, CalculateSliderVolume(value.volume));
        }
    }

    public void SetSFXEntityVolume(VolumeSettings value)
    {
        if (value.muted)
        {
            audioMixer.SetFloat(sfxEntityVolumeKey, CalculateSliderVolume(0f));
        }
        else
        {
            audioMixer.SetFloat(sfxEntityVolumeKey, CalculateSliderVolume(value.volume));
        }   
    }

    public void SetSFXUIVolume(VolumeSettings value)
    {
        if (value.muted)
        {
            audioMixer.SetFloat(sfxUIVolumeKey, CalculateSliderVolume(0f));
        }
        else
        {
            audioMixer.SetFloat(sfxUIVolumeKey, CalculateSliderVolume(value.volume));
        }
        
    }

    private float CalculateSliderVolume(float sliderValue)
    {
        if (sliderValue <= 0.0001f)
        {
            return -80f;
        }
        else
        {
            return Mathf.Log10(sliderValue) * 20f;
        }

    }

    // BGM은 계속 재생, 일시 정지 중에 사운드 효과는 멈추지 않도록 하기
    private void Update()
    {
        if (isPaused && bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.Pause(); // BGM 일시 정지
        }
        else if (!isPaused && bgmSource != null && !bgmSource.isPlaying)
        {
            bgmSource.Play(); // BGM 재개
        }
    }
}

[System.Serializable]
public class AudioClipData
{
    public List<AudioClip> clip;
    public float volume;
    public float pitch;

    public AudioClipData(List<AudioClip> clip, float volume, float pitch)
    {
        this.clip = clip;
        this.volume = volume;
        this.pitch = pitch;
    }

    public AudioClip GetRandomAudioClip()
    {
        if (clip.Count == 1)
        {
            return clip[0];
        }
        else
        {
            int randomIndex = Random.Range(0, clip.Count);
            return clip[randomIndex];
        }
    }
}
