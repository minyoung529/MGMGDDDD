using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateClockHand : MonoBehaviour
{
    [SerializeField]
    private float rotateDuration = 1f;

    [SerializeField]
    private bool isClockMove = false;

    // Clock Move
    private float timer = 0f;
    private float rotateTime;
    private int section = 12;

    private void Start()
    {
        rotateTime = rotateDuration / section;
        timer = rotateTime;

        Vector3 eulerAngles = transform.localEulerAngles;
        eulerAngles.y = Random.Range(0f, 360f);
        transform.localEulerAngles = eulerAngles;
    }

    private void Update()
    {
        Vector3 localEuler = transform.localEulerAngles;

        if (isClockMove)
        {
            timer -= Time.deltaTime;

            if (timer < 0f)
            {
                localEuler.y += 360f / section;
                transform.DOLocalRotate(localEuler, rotateTime * 0.8f);
                timer += rotateTime;
            }
        }
        else
        {
            localEuler = transform.localEulerAngles;
            localEuler.y += Time.deltaTime * 360f * (1f / rotateDuration);
            transform.localEulerAngles = localEuler;
        }
    }
}
