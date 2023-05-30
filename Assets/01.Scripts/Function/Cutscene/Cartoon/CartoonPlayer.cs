using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class CartoonPlayer : MonoBehaviour
{
    [SerializeField]
    private float duration = 1f;
    public float Duration => duration;

    [SerializeField]
    private UnityEvent onPlay;

    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void ReadyToPlay()
    {
        gameObject.SetActive(false);
    }

    public void Play()
    {
        gameObject.SetActive(true);
        onPlay?.Invoke();
        image.color = Color.clear;
        image.DOColor(Color.white, 1f);
    }
}
