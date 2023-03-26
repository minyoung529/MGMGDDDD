using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearRotation : MonoBehaviour
{
    [SerializeField] bool isRotate = false;
    [SerializeField] Vector3 dir = Vector3.back;
    private readonly float rotSpeed = 0.7f;
    private float curRotSpeed = 0f;

    [SerializeField]
    private Animator animator;

    private void Awake()
    {
        if (isRotate) StartGear();
    }

    public void StartGear()
    {
        isRotate = true;

        if (animator == null)
        {
            DOTween.To(() => curRotSpeed, (x) => curRotSpeed = x, rotSpeed, 0.6f).SetEase(Ease.InQuad);
            StartCoroutine(RotateGear());
        }
        else
        {
            animator.SetBool("Rotate", true);
        }
    }
    public void StopGear()
    {
        isRotate = false;

        if(animator == null)
        {
            DOTween.To(() => curRotSpeed, (x) => curRotSpeed = x, 0f, 0.5f).SetEase(Ease.InQuad).OnComplete(() =>
            {
                StopCoroutine(RotateGear());
            });
        }
        else
        {
            animator.SetBool("Rotate", false);
        }
    }

    IEnumerator RotateGear()
    {
        while (isRotate)
        {
            yield return null;
            transform.Rotate(dir * curRotSpeed);
        }
    }

}
