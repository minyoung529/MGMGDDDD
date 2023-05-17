using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;
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

    protected override void Awake()
    {
        base.Awake();
        SetAudioSource();

        pool = new ObjectPool<AudioSourceObject>(CreateAudio, OnGetAudio, OnRelease, OnDestroyed, maxSize: 5);

    }

    private void SetAudioSource()
    {
        effectSource = gameObject.AddComponent<AudioSource>();
        effectSource.playOnAwake = false;
        effectSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("BGM")[0];
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

    /// <summary>
    /// Play Effect Sound At position
    /// </summary>
    /// <param name="clip">Audio clip to play</param>
    /// <param name="pos"></param>
    public AudioSourceObject PlayEffect(AudioClip clip, Vector3 pos)
    {
        AudioSourceObject obj = pool.Get();
        AudioSource audio = obj.AudioSource;

        obj.transform.position = pos;
        audio.clip = clip;
        audio.Play();

        obj.SetClipDuration(clip.length);
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
    public void PlayRandomPitch(AudioClip clip, float pitchRange = defaultRandomPitch)
    {
        float randomPitch = Random.Range(-pitchRange, pitchRange);
        effectSource.pitch = 1 + randomPitch;
        effectSource.clip = clip;
        effectSource.Play();
    }

    public void PlayRandomPitch(AudioClip clip, Vector3 pos, float pitchRange = defaultRandomPitch)
    {
        AudioSourceObject obj = PlayEffect(clip, pos);
        float randomPitch = Random.Range(-pitchRange, pitchRange);
        obj.AudioSource.pitch = 1 + randomPitch;
    }


    /// <summary>
    /// Play random effect sound in clip array as random pitch
    /// </summary>
    /// <param name="pitchRange">-pitchRange to pitchRange</param>
    public void PlayRandomPitch(AudioClip[] clips, float pitchRange = defaultRandomPitch)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(-pitchRange, pitchRange);
        effectSource.pitch = 1 + randomPitch;
        effectSource.clip = clips[randomIndex];
        effectSource.Play();
    }
    #endregion

    #region Music

    /// <summary>
    /// Play BGM
    /// </summary>
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    /// <summary>
    /// Fade out and stop music (BGM)
    /// </summary>
    /// <param name="time">fade out duration</param>
    /// <param name="onEndMusic">end callback function</param>
    public void StopMusic(float time, Action onEndMusic = null)
    {
        StartCoroutine(StopMusicCoroutine(time, onEndMusic));
    }

    private IEnumerator StopMusicCoroutine(float time, Action onEndMusic = null)
    {
        float defaultVolume = musicSource.volume;
        float timer = time;
        while (timer >= 0)
        {
            musicSource.volume = timer / time * defaultVolume;
            timer -= Time.deltaTime;
            yield return null;
        }
        musicSource.Stop();
        musicSource.volume = defaultVolume;

        onEndMusic?.Invoke();
    }
    #endregion

    #region SETTING
    /// <summary>
    /// Change volume by audio mixer's group name
    /// </summary>
    /// <param name="groupName">Master, SFX, BGM</param>
    public void SetVolume(string groupName, float volume)
    {
        AudioMixerGroup group = audioMixer.FindMatchingGroups(groupName)[0];
        if (group != null)
        {
            audioMixer.SetFloat(groupName, volume);
        }
        else
        {
            Debug.LogError($"{groupName}�̶� �̸��� �׷��� �ͼ����� ã�� �� �����ϴ�!");
        }
    }
    #endregion
}