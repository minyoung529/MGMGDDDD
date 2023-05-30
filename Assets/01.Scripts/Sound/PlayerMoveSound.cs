using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip walkClip;

    [SerializeField]
    private AudioClip runClip;

    [SerializeField]
    private AudioClip jumpClip;

    [SerializeField]
    private AudioClip landingClip;

    [SerializeField]
    private AudioClip throwClip;

    [SerializeField]
    private AudioClip recallClip;

    public void WalkSound()
    {
        SoundManager.Instance.PlayRandomPitch(walkClip, transform.position, 0.08f, 10f);
    }

    public void RunSound()
    {
        AudioSourceObject obj = SoundManager.Instance.PlayEffect(runClip, transform.position);
        obj.transform.SetParent(transform);
    }

    public void JumpSound()
    {
        AudioSourceObject obj = SoundManager.Instance.PlayEffect(jumpClip, transform.position);
        obj.transform.SetParent(transform);
    }

    public void LandingSound()
    {
        AudioSourceObject obj = SoundManager.Instance.PlayEffect(landingClip, transform.position);
        obj.transform.SetParent(transform);
    }

    public void ThrowSound()
    {
        AudioSourceObject obj = SoundManager.Instance.PlayEffect(throwClip, transform.position);
        obj.transform.SetParent(transform);
    }

    public void RecallSound()
    {
        AudioSourceObject obj = SoundManager.Instance.PlayEffect(recallClip, transform.position);
        obj.transform.SetParent(transform);
    }
}
