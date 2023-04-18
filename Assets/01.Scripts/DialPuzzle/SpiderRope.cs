using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderRope : MonoBehaviour
{
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;
    

    public void Awake()
    {
        ResetSpider();
    }

    public void ResetSpider()
    {
        transform.DOKill();
        transform.position = startPos.position;
    }

    public void StartFalling(float duration)
    {
        transform.DOMoveY(endPos.position.y, duration);
    }
}