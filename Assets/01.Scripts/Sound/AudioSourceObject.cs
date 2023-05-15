using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AudioSourceObject : MonoBehaviour
{
    private IObjectPool<AudioSourceObject> managedPool;

    private AudioSource audioSource;
    public AudioSource AudioSource => audioSource;

    private float length = 0f;
    private bool isPlaying = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!isPlaying) return;

        length -= Time.deltaTime;

        if (length < 0f)
        {
            DisableAudioSource();
        }
    }

    public void SetManagedPool(IObjectPool<AudioSourceObject> managed)
    {
        managedPool = managed;
    }

    private void DisableAudioSource()
    {
        managedPool.Release(this);
        isPlaying = false;
    }

    public void SetClipDuration(float length)
    {
        this.length = length;
        isPlaying = true;
    }

    public void Stop()
    {
        DisableAudioSource();
    }
}
