using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour
{
    [SerializeField] private float speed = 60f;
    [SerializeField] private float speedDuration = 1f;
    private float curSpeed = 0f;

    private Sequence seq;

    private void Start()
    {
        StartPropeller();
    }

    private void Update()
    {
        if (curSpeed > 0.01f)
        {
            Vector3 eulerAngles = transform.localEulerAngles;
            eulerAngles.z += curSpeed * Time.deltaTime;
            transform.DOLocalRotate(eulerAngles, 0f);
        }
    }

    private void StartPropeller()
    {
        seq = DOTween.Sequence();
        seq.Append(DOTween.To(() => curSpeed, (x) => curSpeed = x, speed, speedDuration));
    }

    private void StopPropeller()
    {
        seq = DOTween.Sequence();
        seq.Append(DOTween.To(() => curSpeed, (x) => curSpeed = x, 0f, speedDuration));
    }

    public void Arrive(bool isDown)
    {
        if (isDown)
        {
            StopPropeller();
        }
    }

    public void Depart()
    {
        if (curSpeed < 0.01f)
            StartPropeller();
    }

    private void OnDestroy()
    {
        seq.Kill();
    }
}
