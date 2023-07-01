using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    BGM,
    SFX,
    Count
}

public class PlaySound : MonoBehaviour
{
    [SerializeField]
    private AudioClip audioClip;

    [SerializeField]
    private SoundType soundType;

    [SerializeField]
    private bool playOnAwake;

    [SerializeField]
    private bool loop;

    [SerializeField]
    [Tooltip("If this variable is true, sound will play at object's position")]
    private bool isPositioning = false;

    [SerializeField]
    [Tooltip("If this variable is true, sound object will be a child of this object")]
    private bool isChildToTransform = false;

    [SerializeField]
    private float volume = 1f;

    [SerializeField]
    private bool forceLoadVolume = false;

    private AudioSourceObject audioSourceObject;

    private IEnumerator Start()
    {
        yield return new WaitForFixedUpdate();

        if (playOnAwake)
        {
            Play();
        }
    }

    [ContextMenu("Play")]
    public void Play()
    {
        if (audioClip == null) return;

        switch (soundType)
        {
            case SoundType.BGM:
                if (forceLoadVolume)
                {
                    SoundManager.Instance.LoadVolumeSmooth();
                }
                SoundManager.Instance.PlayMusic(audioClip);
                break;

            case SoundType.SFX:
                {
                    if (isPositioning)
                    {
                        audioSourceObject = SoundManager.Instance.PlayEffect(audioClip, transform.position, volume, loop);
                    }
                    else
                    {
                        audioSourceObject = SoundManager.Instance.PlayEffect(audioClip, Vector3.zero, volume, loop);
                    }
                    if (isChildToTransform)
                    {
                        audioSourceObject.transform.SetParent(transform);
                        audioSourceObject.transform.localPosition = Vector3.zero;
                    }

                }
                break;
        }
    }

    public void Stop()
    {
        switch (soundType)
        {
            case SoundType.BGM:
                SoundManager.Instance.StopBGM();
                break;

            case SoundType.SFX:
                audioSourceObject?.Stop();
                break;
        }
    }

    public void SetVolume(float volume)
    {
        audioSourceObject.AudioSource.volume = volume;
    }
}
