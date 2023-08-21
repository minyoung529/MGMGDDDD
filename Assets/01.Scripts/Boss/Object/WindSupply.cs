using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class WindSupply : MonoBehaviour
{
    [SerializeField] private float delayTime = 4f;
    [SerializeField] private Transform sticky;

    private Vector3 originScale = Vector3.zero;
    public void Trigger()
    {
        originScale = sticky.transform.localScale;
        StartCoroutine(AddScale());
    }

    private IEnumerator AddScale()
    {
        yield return new WaitForSeconds(delayTime);
        sticky.DOScale(originScale * 6f, 1.5f).SetEase(Ease.InBounce);
    }

}
