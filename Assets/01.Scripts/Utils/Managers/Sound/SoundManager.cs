using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class SoundManager : MonoSingleton<SoundManager>
{
	// Audio players components.
	private AudioSource effectSource;
    private AudioSource musicSource;

	[SerializeField] private GameObject soundPref;
	[SerializeField] private AudioMixer audioMixer;

	[SerializeField] private const float defaultRandomPitch = 0.05f;

    protected override void Awake()
    {
		base.Awake();
		SetAudioSource();
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

    #region ????? ???
    //????? ???
    public void PlayEffect(AudioClip clip)
	{
		effectSource.clip = clip;
		effectSource.Play();
	}

	//??????? ?????? ?????? ??? (????? ? ????????? ????)
	public void PlayEffect(AudioClip clip, Vector3 pos)
    {
		GameObject obj = Instantiate(soundPref);
		AudioSource audio = obj.GetComponent<AudioSource>();
		obj.transform.position = pos;
		audio.clip = clip;
		obj.SetActive(true);
		audio.Play();
		RemovePrefab(obj, clip.length);
	}

	private IEnumerator RemovePrefab(GameObject obj, float time)
	{
		yield return new WaitForSeconds(time);
		Destroy(obj);
	}

	#region ???????
	public void PlayRandomPitch(AudioClip clip, float pitchRange = defaultRandomPitch)
	{
		float randomPitch = Random.Range(-pitchRange, pitchRange);
		effectSource.pitch = 1 + randomPitch;
		effectSource.clip = clip;
		effectSource.Play();
	}

	//????? ????? ????? ???????? ??????? ???
	public void PlayRandomPitch(AudioClip[] clips, float pitchRange = defaultRandomPitch)
	{
		int randomIndex = Random.Range(0, clips.Length);
		float randomPitch = Random.Range(-pitchRange, pitchRange);
		effectSource.pitch = 1 + randomPitch;
		effectSource.clip = clips[randomIndex];
		effectSource.Play();
	}
	#endregion
	#endregion

	#region ????
	//???? ???
	public void PlayMusic(AudioClip clip)
	{
		musicSource.clip = clip;
		musicSource.Play();
	}

	/// <summary>
	/// ???? ??????? ?????? ????? ?????? ????? ?????? ??????
	/// </summary>
	/// <param name="time">?????? ?????? ?????????? ????? ????</param>
	/// <param name="action">?????? ????? ?? ????? ??? ??)StopMusic(1f, PlayMusic(musicClip))</param>
	public void StopMusic(float time, Action action = null)
	{
		StartCoroutine(StopMusicCoroutine(time));
		action?.Invoke();
	}

	private IEnumerator StopMusicCoroutine(float time)
    {
		float defaultVolume = musicSource.volume;
		float timer = time;
		while(timer >= 0)
        {
			musicSource.volume = timer / time * defaultVolume;
			timer -= Time.deltaTime;
			yield return null;
        }
		musicSource.Stop();
		musicSource.volume = defaultVolume;
		yield break;
	}
	#endregion

	#region ????
	/// <summary>
	/// ??????? ??? ????? ????? ????? ????? a?? ?????? ????
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
			Debug.LogError($"{groupName}??? ????? ????? ??????? a?? ?? ???????!");
    }
    #endregion
}