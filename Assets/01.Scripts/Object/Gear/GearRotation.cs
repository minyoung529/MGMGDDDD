using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearRotation : MonoBehaviour
{
    [SerializeField] bool playOnAwake = false;
    [SerializeField] Vector3 dir = Vector3.back;

    private readonly float rotSpeed = 0.7f;
    private float curRotSpeed = 0f;
    private bool isRotate = false;
    //private Animator animator;

    private void Awake()
    {
      //  animator = GetComponent<Animator>();
        if (playOnAwake) StartGear();
    }

    public void StartGear()
    {
        if (isRotate) return;
        isRotate = true;
        StartCoroutine(RotateGear());
        //if (animator == null)
        //{
        //    DOTween.To(() => curRotSpeed, (x) => curRotSpeed = x, rotSpeed, 0.6f).SetEase(Ease.InQuad);
        //}
        //else
        //{
        //    animator.SetBool("Rotate", true);
        //}
    }
    public void StopGear()
    {
        isRotate = false;

        Debug.Log("StopGear");
           // StopCoroutine(RotateGear());
        //DOTween.To(() => curRotSpeed, (x) => curRotSpeed = x, 0f, 0.5f).SetEase(Ease.InQuad).OnComplete(() =>
        //{
        //});
        //if (animator == null)
        //{
           
        //}
        //else
        //{
        //    animator.SetBool("Rotate", false);
        //}
    }

    IEnumerator RotateGear()
    {
        while (isRotate)
        {
            transform.Rotate(dir * curRotSpeed);
        }
            yield return null;
    }

}
