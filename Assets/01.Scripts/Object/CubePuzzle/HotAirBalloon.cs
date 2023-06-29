using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HotAirBalloon : MonoBehaviour
{
    [SerializeField]
    private Vector3[] recordedPositions;

    [SerializeField]
    private ParticleSystem particle;

    [SerializeField]
    private UnityEvent onShow;

    [SerializeField]
    private UnityEvent onReset;

    void Start()
    {
        particle.transform.SetParent(transform.parent);
        gameObject?.SetActive(false);
    }

    public void MoveCubeToThis(int index, bool isFinal)
    {
        if (gameObject.activeSelf) return;

        gameObject?.SetActive(true);
        transform.localPosition = recordedPositions[index];
        particle?.Play();
        onShow?.Invoke();
    }

    public void OnReset()
    {
        if (!gameObject.activeSelf) return;

        particle?.Play();
        gameObject?.SetActive(false);
        onReset?.Invoke();
    }

    public void OffParticle()
    {
        particle.gameObject.SetActive(false);
    }
}
