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
        SoundManager.Instance.PlayEffect(runClip, transform.position);
    }

    public void JumpSound()
    {
        SoundManager.Instance.PlayEffect(jumpClip, transform.position);
    }

    public void LandingSound()
    {
        SoundManager.Instance.PlayEffect(landingClip, transform.position);
    }
    
    public void ThrowSound()
    {
        SoundManager.Instance.PlayEffect(throwClip, transform.position);
    }

    public void RecallSound()
    {
        SoundManager.Instance.PlayEffect(recallClip, transform.position);
    }
}
