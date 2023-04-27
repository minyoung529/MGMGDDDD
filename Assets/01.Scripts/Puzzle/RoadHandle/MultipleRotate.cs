using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleRotate : MonoBehaviour
{
    [SerializeField]
    private int count = 10;

    [SerializeField]
    private float targetY;

    [SerializeField]
    private float duration = 1f;

    private bool isRotating = false;

    private float rotateDegree;
    private float remainDegrees;

    private void Update()
    {
        if (!isRotating) return;

        Vector3 euler = transform.localEulerAngles;
        euler.y = rotateDegree;

        transform.localEulerAngles = euler;
    }

    [ContextMenu("Rotate")]
    public void Rotate()
    {
        isRotating = true;
        CalculateDegrees();

        DOTween.To(() => rotateDegree, (x) => rotateDegree = x, remainDegrees, duration)
            .SetEase(Ease.OutElastic)
            .OnComplete(() => isRotating = false);
    }

    private void CalculateDegrees()
    {
        rotateDegree = transform.localEulerAngles.y;
        remainDegrees = 360f * count + (targetY-transform.localEulerAngles.y) + transform.localEulerAngles.y;
    }   
}
