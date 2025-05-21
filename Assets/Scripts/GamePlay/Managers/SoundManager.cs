using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using static Unity.VisualScripting.Member;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public Transform SoundParent;
    public int GlobalVolume;
    [SerializeField] private SoundDataStorage soundDataStorage;
    [SerializeField] private int poolSize = 10;

    private Dictionary<string, AudioClipData> soundClips = new();
    private Queue<AudioSource> sfxPool = new();
    private List<AudioSource> inUse = new(); // 사용중인 경우

    // 볼륨 제어 부분
    public AudioMixerGroup sfxGroup;
    public AudioMixerGroup bgmGroup;
    public AudioMixer audioMixer;

    private AudioSource bgmSource;
    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        InitSounds();
        InitSFXPool();
        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayBGM("MainBGM");
        SoundParent = GameObject.Find("SoundParent").transform;
    }

    private void InitSounds()
    {
        foreach (var entry in soundDataStorage.sounds)
        {
            if (!soundClips.ContainsKey(entry.key))
                soundClips.Add(entry.key, new AudioClipData(entry.clip, entry.volume));
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
    public void PlaySFX(string key, Vector3? pos = null)
    {
        if (!soundClips.TryGetValue(key, out var data)) return;

        if (sfxPool.Count == 0) return;

        var source = sfxPool.Dequeue();
        inUse.Add(source);

        if (pos.HasValue)
        {
            source.transform.position = pos.Value;
            source.spatialBlend = 1f; // 공간 음향 적용
        }
        else
        {
            source.spatialBlend = 0f; // 공간 음향 적용 안함.
        }

        source.outputAudioMixerGroup = sfxGroup; // 볼륨 그룹 나누기
        source.volume = data.Volume;
        source.clip = data.Clip;
        source.Play();

        StartCoroutine(ReturnToPoolWhenDone(source));
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
        bgmSource.clip = data.Clip;
        bgmSource.volume = data.Volume;
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
        else
        {

        }
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGMVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
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

    public class AudioClipData
    {
        public AudioClip Clip;
        public float Volume;

        public AudioClipData(AudioClip clip, float volume)
        {
            Clip = clip;
            Volume = volume;
        }
    }
}
