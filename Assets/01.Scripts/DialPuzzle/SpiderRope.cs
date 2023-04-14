using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderRope : MonoBehaviour
{
    [SerializeField] float downDuration = 2f;
    [SerializeField] float upDuration = 1f;

    private float arrivePosY = 321f;
    private float startPosY = 401f;

    private void Awake()
    {
        ResetSpider();
    }

    public void SetDownDuration(float duration)
    {
        downDuration= duration;
    }

    public void ResetSpider()
    {
        MoveSpider(startPosY, upDuration);
    }
    public void ResetSpider(float duration)
    {
        MoveSpider(startPosY, duration);
    }

    public void FallSpider()
    {
        MoveSpider(arrivePosY, downDuration);
    }

    public void MoveSpider(float endPosY, float moveDuration)
    {
        transform.DOMoveY(endPosY, moveDuration);
    }

    // 시간이 다 됐을 때 맨 아래
    // 길이 나누기 시간으로 ++


}