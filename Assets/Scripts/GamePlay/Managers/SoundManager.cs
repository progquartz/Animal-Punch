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
    private SoundDataStorage soundDataStorage;
    [SerializeField] private int poolSize = 10;

    private Dictionary<string, AudioClipData> soundClips = new();
    private Queue<AudioSource> sfxPool = new();
    private List<AudioSource> inUse = new(); // 사용중인 경우
    private List<AudioSource> loopingSources = new(); // 루프 중인 SFX 추적용


    // 볼륨 제어 부분
    public AudioMixerGroup masterAudioGroup;
    public AudioMixerGroup sfxEntityGroup;
    public AudioMixerGroup sfxSFXGroup;
    public AudioMixerGroup sfxUIGroup;
    public AudioMixerGroup bgmGroup;
    public AudioMixer audioMixer;

    private AudioSource bgmSource;
    private bool isPaused = false;

    private readonly string masterVolumeKey = "MasterVolume";
    private readonly string bgmVolumeKey = "BGMVolume";
    private readonly string sfxEntityVolumeKey = "SFXEntityVolume";
    private readonly string sfxSFXVolumeKey = "SFXSFXVolume";
    private readonly string sfxUIVolumeKey = "SFXUIVolume";
    

    protected override void Init()
    {
        base.Init();
        InitSoundParent();
        InitSounds();
        InitSFXPool();
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(ApplyVolumeWithDelay()); 
        PlayBGM("MainBGM");

        Debug.Log("SoundManager Init");
    }

    private void InitSounds()
    {
        soundDataStorage = DataManager.Instance.SoundStorage;

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

    private void InitSoundParent()
    {
        if (SoundParent == null)
        {
            GameObject parentObj = GameObject.Find("SoundParent");

            if (parentObj == null)
            {
                parentObj = new GameObject("SoundParent");
                DontDestroyOnLoad(parentObj); // 사운드 부모도 유지
            }

            SoundParent = parentObj.transform;
        }
    }

    /// <summary>
    /// 사운드 재생, 단 캐릭터에게서 나는 소리일 경우에는 pos를 지정.
    /// UI 효과음 등은, 효과음을 지정하지 않아도 됨.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="pos"></param>
    public void PlaySFX(string key, AudioType audioType, Vector3? pos = null, bool isLoop = false)
    {
        if (!soundClips.TryGetValue(key, out var data))
        {
            Debug.LogWarning($"{key}값을 가지는 SFX가 발견되지 않았습니다.");
            return;
        }

        if (sfxPool.Count == 0)
        {
            Debug.Log("SFXPool이 가득 찼습니다.");
            return;
        }

        var sfxSource = sfxPool.Dequeue();
        inUse.Add(sfxSource);

        if (pos.HasValue)
        {
            sfxSource.transform.position = pos.Value;
            sfxSource.spatialBlend = 1f;
        }
        else
        {
            sfxSource.spatialBlend = 0f;
        }

        switch (audioType)
        {
            case AudioType.Entity: sfxSource.outputAudioMixerGroup = sfxEntityGroup; break;
            case AudioType.Bgm: sfxSource.outputAudioMixerGroup = bgmGroup; break;
            case AudioType.UI: sfxSource.outputAudioMixerGroup = sfxUIGroup; break;
            case AudioType.SFX: sfxSource.outputAudioMixerGroup = sfxSFXGroup; break;
            default: sfxSource.outputAudioMixerGroup = sfxEntityGroup; break;
        }

        sfxSource.volume = data.volume;
        sfxSource.clip = data.GetRandomAudioClip();
        sfxSource.pitch = data.pitch;
        sfxSource.loop = isLoop;
        sfxSource.Play();

        if (isLoop)
        {
            loopingSources.Add(sfxSource);
        }
        else
        {
            StartCoroutine(ReturnToPoolWhenDone(sfxSource));
        }
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
            DontDestroyOnLoad(bgmSource);
            bgmSource.loop = loop;
        }

        bgmSource.outputAudioMixerGroup = bgmGroup; // 볼륨 그룹 나누기
        bgmSource.clip = data.GetRandomAudioClip();
        bgmSource.volume = data.volume;
        bgmSource.pitch = data.pitch;
        bgmSource.Play();
    }
    public void StopLoopSFX(string key)
    {
        for (int i = loopingSources.Count - 1; i >= 0; i--)
        {
            var source = loopingSources[i];
            if (source.clip != null && soundClips.TryGetValue(key, out var data) && data.clip.Contains(source.clip))
            {
                source.Stop();
                source.loop = false;
                source.clip = null;
                loopingSources.RemoveAt(i);
                inUse.Remove(source);
                sfxPool.Enqueue(source);
            }
        }
    }


    private IEnumerator ReturnToPoolWhenDone(AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);
        source.clip = null;
        source.loop = false; // 루프 초기화
        inUse.Remove(source);
        sfxPool.Enqueue(source);
    }

    public void StopBGM() => bgmSource?.Stop();

    // 씬이 로드될 때 모든 SFX를 멈추기
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SoundParent == null || SoundParent.gameObject.scene.name != "DontDestroyOnLoad")
        {
            InitSoundParent();
        }

        StopAllSFX(); // SFX 정리
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

    private IEnumerator ApplyVolumeWithDelay()
    {
        yield return null; // 한 프레임 대기 (혹은 WaitForSeconds(0.05f))
        ApplyVolumeFromSettings();
    }

    public void ApplyVolumeFromSettings()
    {
        var settings = SettingsManager.Instance;

        SetMasterVolume(settings.masterVolume);
        SetBGMVolume(settings.bgmVolume);
        SetSFXEntityVolume(settings.entityVolume);
        SetSFXUIVolume(settings.uiVolume);
        SetSFXSFXVolume(settings.sfxVolume);
        
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

    public void SetSFXSFXVolume(VolumeSettings value)
    {
        if (value.muted)
        {
            audioMixer.SetFloat(sfxSFXVolumeKey, CalculateSliderVolume(0f));
        }
        else
        {
            audioMixer.SetFloat(sfxSFXVolumeKey, CalculateSliderVolume(value.volume));
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
