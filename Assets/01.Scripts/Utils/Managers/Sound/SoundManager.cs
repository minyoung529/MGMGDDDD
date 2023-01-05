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

    private void Awake()
    {
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

    #region 효과음 재생
    //효과음 재생
    public void PlayEffect(AudioClip clip)
	{
		effectSource.clip = clip;
		effectSource.Play();
	}

	//효과음을 지정한 장소에서 재생 (나중에 풀 이용하는걸로 수정)
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

	#region 랜덤재생
	public void PlayRandomPitch(AudioClip clip, float pitchRange = defaultRandomPitch)
	{
		float randomPitch = Random.Range(-pitchRange, pitchRange);
		effectSource.pitch = 1 + randomPitch;
		effectSource.clip = clip;
		effectSource.Play();
	}

	//효과음 클립의 피치를 랜덤으로 변경해서 재생
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

	#region 음악
	//음악 재생
	public void PlayMusic(AudioClip clip)
	{
		musicSource.clip = clip;
		musicSource.Play();
	}

	/// <summary>
	/// 현재 재생중인 음악의 소리를 서서히 줄이며 음악을 종료함
	/// </summary>
	/// <param name="time">음악이 완전히 종료될때까지 걸리는 시간</param>
	/// <param name="action">음악이 종료된 후 실행될 함수 예)StopMusic(1f, PlayMusic(musicClip))</param>
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

	#region 설정
	/// <summary>
	/// 믹서에서 해당 그룹의 이름을 이용해 그룹을 찾아 볼륨을 변경
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
			Debug.LogError($"{groupName}이란 이름의 그룹을 믹서에서 찾을 수 없습니다!");
    }
    #endregion
}