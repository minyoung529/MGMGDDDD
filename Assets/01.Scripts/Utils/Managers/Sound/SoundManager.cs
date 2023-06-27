using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;
using DG.Tweening;
using Random = UnityEngine.Random;

public class SoundManager : MonoSingleton<SoundManager>
{
    // Audio players components.
    private AudioSource effectSource;
    private AudioSource musicSource;

    [SerializeField] private AudioSourceObject audioSourcePrefab;
    private IObjectPool<AudioSourceObject> pool;

    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private const float defaultRandomPitch = 0.05f;

    public AudioMixer AudioMixer => audioMixer;

    protected override void Awake()
    {
        base.Awake();
        SetAudioSource();

        pool = new ObjectPool<AudioSourceObject>(CreateAudio, OnGetAudio, OnRelease, OnDestroyed, maxSize: 5);

        CutSceneManager.Instance.AddStartCutscene(MuteBGM);
        CutSceneManager.Instance.AddEndCutscene(LoadVolumeSmooth);

        SceneController.ListeningEnter(StopBGM);
    }

    private void SetAudioSource()
    {
        effectSource = gameObject.AddComponent<AudioSource>();
        effectSource.playOnAwake = false;
        effectSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("BGM")[0];
        musicSource.loop = true;
    }

    private void Start()
    {
        LoadVolume();
    }

    #region EFFECT SOUND

    /// <summary>
    /// Play Effect Sound
    /// </summary>
    /// <param name="clip">Audio clip to play</param>
    public void PlayEffect(AudioClip clip)
    {
        effectSource.clip = clip;
        effectSource.Play();
    }

    public void PlayEffect(AudioClip clip, float volumeScale)
    {
        effectSource.clip = clip;
        effectSource.volume = volumeScale;
        effectSource.Play();
    }


    /// <summary>
    /// Play Effect Sound At position
    /// </summary>
    /// <param name="clip">Audio clip to play</param>
    /// <param name="pos"></param>
    public AudioSourceObject PlayEffect(AudioClip clip, Vector3 pos, float volumeScale = 1f, bool loop = false)
    {
        if (clip == null) return null;
        AudioSourceObject obj = pool.Get();
        AudioSource audio = obj.AudioSource;

        if (pos != Vector3.zero)
        {
            obj.transform.position = pos;
            audio.spatialBlend = 1f;
        }
        else
        {
            audio.spatialBlend = 0f;
        }
        audio.clip = clip;
        audio.volume = volumeScale;
        audio.loop = loop;
        audio.Play();
        audio.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];

        if (!loop) obj.SetClipDuration(clip.length);
        return obj;
    }
    #endregion

    #region POOL
    private AudioSourceObject CreateAudio()
    {
        AudioSourceObject obj = Instantiate(audioSourcePrefab);
        obj.SetManagedPool(pool);
        obj.transform.SetParent(transform);
        return obj;
    }

    public void OnGetAudio(AudioSourceObject obj)
    {
        obj.gameObject.SetActive(true);
    }

    public void OnRelease(AudioSourceObject obj)
    {
        obj.AudioSource.Stop();
        obj.AudioSource.clip = null;
        obj.gameObject.SetActive(false);

        if (SoundManager.Instance)
        {
            obj.transform.SetParent(SoundManager.Instance.transform);
        }
    }

    public void OnDestroyed(AudioSourceObject obj)
    {
        Destroy(obj.gameObject);
    }
    #endregion

    #region Random
    /// <summary>
    /// Play effect sound as random pitch
    /// </summary>
    /// <param name="pitchRange">-pitchRange to pitchRange</param>
    public void PlayRandomPitch(AudioClip clip, float pitchRange = defaultRandomPitch, float volume = 1f)
    {
        float randomPitch = Random.Range(-pitchRange, pitchRange);
        effectSource.pitch = 1 + randomPitch;
        effectSource.clip = clip;
        effectSource.volume = volume;
        effectSource.Play();
    }

    public void PlayRandomPitch(AudioClip clip, Vector3 pos, float pitchRange = defaultRandomPitch, float volume = 1f)
    {
        if (clip == null) return;

        AudioSourceObject obj = PlayEffect(clip, pos, volume);
        float randomPitch = Random.Range(-pitchRange, pitchRange);
        obj.AudioSource.pitch = 1 + randomPitch;
    }


    /// <summary>
    /// Play random effect sound in clip array as random pitch
    /// </summary>
    /// <param name="pitchRange">-pitchRange to pitchRange</param>
    public void PlayRandomPitch(AudioClip[] clips, float pitchRange = defaultRandomPitch, float volume = 1f)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(-pitchRange, pitchRange);
        effectSource.pitch = 1 + randomPitch;
        effectSource.PlayOneShot(clips[randomIndex], volume);
        effectSource.PlayOneShot(clips[randomIndex], volume);
    }
    #endregion

    #region Music

    /// <summary>
    /// Play BGM
    /// </summary>
    public void PlayMusic(AudioClip clip, bool loop = false)
    {
        musicSource.clip = clip;
        musicSource.Play();
        musicSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("BGM")[0];
    }
    #endregion

    #region SETTING
    /// <summary>
    /// Change volume by audio mixer's group name
    /// </summary>
    /// <param name="groupName">Master, SFX, BGM</param>
    public void SetVolume(string groupName, float volume, float duration = 0f, Action onComplete = null)
    {
        AudioMixerGroup group = audioMixer.FindMatchingGroups(groupName)[0];
        if (group != null)
        {
            float calculatedVolume = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20;

            if (duration == 0f)
            {
                audioMixer.SetFloat($"{groupName}Volume", calculatedVolume);
            }
            else
            {
                audioMixer.DOSetFloat($"{groupName}Volume", calculatedVolume, duration).OnComplete(() => onComplete?.Invoke());
            }
        }
        else
        {
            Debug.LogError($"{groupName}�̶� �̸��� �׷��� �ͼ����� ã�� �� �����ϴ�!");
        }
    }

    public void StopBGM()
    {
        if (SoundManager.Instance.musicSource.clip != null)
        {
            SetVolume("BGM", 0f, 2f, ResetBGM);
        }
    }

    private void ResetBGM()
    {
        SoundManager.Instance.musicSource.clip = null;
        LoadVolume();
    }

    public void MuteSound()
    {
        SetVolume("Master", 0f, 1f);
    }

    public void MuteBGM()
    {
        SetVolume("BGM", 0f, 1f);
    }

    public void LoadVolume(float duration = 0f)
    {
        if (SaveSystem.CurSaveData == null) SaveSystem.Load();
        SaveData data = SaveSystem.CurSaveData;

        SetVolume("BGM", data.bgmVolume, duration);
        SetVolume("SFX", data.sfxVolume, duration);
        SetVolume("Master", data.masterVolume, duration);
    }

    public void LoadVolumeSmooth()
    {
        LoadVolume(1f);
    }

    #endregion

    private void OnDestroy()
    {
        CutSceneManager.Instance?.RemoveStartCutscene(MuteSound);
        CutSceneManager.Instance?.RemoveEndCutscene(LoadVolumeSmooth);

        SceneController.StopListeningEnter(StopBGM);
    }
}