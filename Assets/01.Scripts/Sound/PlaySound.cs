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
    [Tooltip("If this variable is true, sound will play at object's position")]
    private bool isPositioning = false;

    public void Play()
    {
        if (audioClip == null) return;

        switch(soundType)
        {
            case SoundType.BGM:
                SoundManager.Instance.PlayMusic(audioClip);
                break;

            case SoundType.SFX:
                {
                    if (isPositioning)
                    {
                        SoundManager.Instance.PlayEffect(audioClip, transform.position);
                    }
                    else
                    {
                        SoundManager.Instance.PlayEffect(audioClip);
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
                SoundManager.Instance.StopMusic(2f);
                break;
        }
    }
}
