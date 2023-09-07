using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip walkSound;

    [SerializeField]
    private AudioClip catchingPetSound;

    [SerializeField]
    private AudioClip findPlayerSound;

    public void PlayWalkSound()
    {
        SoundManager.Instance.PlayEffect(walkSound, transform.position);
    }

    public void PlayCatchingPetSound()
    {
        SoundManager.Instance.PlayEffect(catchingPetSound, transform.position, 2f);
    }

    public void PlayFindPlayerSound()
    {
        SoundManager.Instance.PlayEffect(findPlayerSound, transform.position);
    }
}
