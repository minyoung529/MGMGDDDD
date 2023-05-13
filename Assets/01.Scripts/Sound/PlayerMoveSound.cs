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

    public void WalkSound()
    {
        SoundManager.Instance.PlayRandomPitch(walkClip, transform.position);
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
}
