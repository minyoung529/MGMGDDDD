using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balance : MonoBehaviour
{
    [SerializeField] private float maxRotation = 140f;
    [SerializeField] private float minRotation = 50f;
    [SerializeField] private float rotationTime = 0.5f;

    [SerializeField] private BalanceFloor leftBalance;
    [SerializeField] private BalanceFloor rightBalance;

    [SerializeField] private Transform leftAnchor;
    [SerializeField] private Transform rightAnchor;
    [SerializeField] private Transform leftPillar;
    [SerializeField] private Transform rightPillar;

    private Transform pillar;

    private void Awake()
    {
        pillar = transform.GetChild(0);
    }

    private void Start()
    {
        Debug.Log(leftBalance.GetWeight);
        Debug.Log(rightBalance.GetWeight);
    }

    public void CompareWeight()
    {
        float val = rightBalance.GetWeight - leftBalance.GetWeight;
        RotateWeight(val);
    }

    private void RotateWeight(float value)
    {
        float xVal = Mathf.Clamp(value, minRotation, maxRotation);
        Quaternion q = Quaternion.Euler(transform.rotation.x, transform.rotation.y, xVal);

        pillar.DOKill();
        pillar.DOLocalRotateQuaternion(q, rotationTime);

        leftPillar.DOMove(leftAnchor.position, rotationTime);
        rightPillar.DOMove(rightAnchor.position, rotationTime);
    }
   
}
