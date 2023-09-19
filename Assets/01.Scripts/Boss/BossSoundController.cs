using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSoundController : MonoBehaviour
{
    [SerializeField]
    private AudioClip defaultBGM;

    [SerializeField]
    private AudioClip chaseBGM;

    [SerializeField]
    private AudioClip warningSFX;

    private bool isDefault = true;

    private void Start()
    {
        isDefault = false;
        SetDefaultBGM();
    }

    public void SetDefaultBGM()
    {
        if (isDefault) return;

        isDefault = true;
        SoundManager.Instance.PlayMusic(defaultBGM);
    }

    public void SetChaseBGM()
    {
        if (!isDefault) return;

        isDefault = false;

        SoundManager.Instance.PlayMusic(chaseBGM);
    }

    public void PlayWarningSFX()
    {
        AudioSourceObject soundObj = SoundManager.Instance.PlayEffect(warningSFX, transform.position, 1f);
        soundObj.AudioSource.spatialBlend = 0f;
    }
}
