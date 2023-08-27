using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    [SerializeField]
    private AudioClip pickupClip;

    [SerializeField]
    private UnityEvent onPlayWalkSound;

    [SerializeField]
    private UnityEvent onPlayRunSound;

    [SerializeField]
    private UnityEvent onPlayLandingSound;

    public void WalkSound()
    {
        SoundManager.Instance.PlayRandomPitch(walkClip, transform.position, 0.08f, 10f);
        onPlayWalkSound?.Invoke();
    }

    public void PickupSound()
    {
        AudioSourceObject obj = SoundManager.Instance.PlayEffect(pickupClip, transform.position);
        obj?.transform.SetParent(transform);
    }

    public void RunSound()
    {
        AudioSourceObject obj = SoundManager.Instance.PlayEffect(runClip, transform.position);
        obj?.transform.SetParent(transform);
        onPlayRunSound?.Invoke();
    }

    public void JumpSound()
    {
        AudioSourceObject obj = SoundManager.Instance.PlayEffect(jumpClip, transform.position, 0.7f);
        obj?.transform.SetParent(transform);
    }

    public void LandingSound()
    {
        AudioSourceObject obj = SoundManager.Instance.PlayEffect(landingClip, transform.position);
        obj?.transform.SetParent(transform);
        onPlayLandingSound?.Invoke();
    }

    public void ThrowSound()
    {
        AudioSourceObject obj = SoundManager.Instance.PlayEffect(throwClip, transform.position);
        obj?.transform.SetParent(transform);
    }

    public void RecallSound()
    {
        AudioSourceObject obj = SoundManager.Instance.PlayEffect(recallClip, transform.position);
        obj?.transform.SetParent(transform);
    }
}
