using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearRotation : MonoBehaviour
{
    [SerializeField] bool isRotate = false;
    [SerializeField] bool isOil = false;
    private readonly float rotSpeed = 0.35f;
    private float curRotSpeed = 0f;

    [SerializeField]
    private Animator animator;

    private void Awake()
    {
        if (isRotate)
        {
            StartGear();
        }
    }
    public void StartGear()
    {
        if (!isOil) return;
        isRotate = true;

        //if (animator)
        //{
        //    animator.SetBool("Rotate", true);
        //}
        //else
        //{ 
        DOTween.To(() => curRotSpeed, (x) => curRotSpeed = x, rotSpeed, 0.6f).SetEase(Ease.InQuad);
        StartCoroutine(RotateGear());
        //}
    }
    public void StopGear()
    {
        DOTween.To(() => curRotSpeed, (x) => curRotSpeed = x, 0f, 0.5f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            isRotate = false;
            StopCoroutine(RotateGear());
        });
    }


    IEnumerator RotateGear()
    {
        while (isRotate)
        {
            yield return null;
            transform.Rotate(Vector3.back * curRotSpeed);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.OIL_BULLET_TAG))
        {
            isOil = true;
            StartGear();
        }
    }
}
