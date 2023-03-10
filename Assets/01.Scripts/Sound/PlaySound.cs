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

    public void Play()
    {
        switch(soundType)
        {
            case SoundType.BGM:
                SoundManager.Instance.PlayMusic(audioClip);
                break;

            case SoundType.SFX:
                SoundManager.Instance.PlayEffect(audioClip);
                break;
        }
    }

    public void Stop()
    {
        switch (soundType)
        {
            case SoundType.BGM:
                SoundManager.Instance.StopMusic(2f);
                break;
        }
    }
}
