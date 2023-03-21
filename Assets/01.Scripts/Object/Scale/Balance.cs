using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Balance : MonoBehaviour
{
    [SerializeField] private float maxRotation = 140f;
    [SerializeField] private float minRotation = 50f;

    [SerializeField] private BalanceFloor leftBalance;
    [SerializeField] private BalanceFloor rightBalance;

    [SerializeField] private Transform pillar;

    private float rotationSpeed = 2f;

    public void CompareWeight()
    {
        float val = rightBalance.GetWeight - leftBalance.GetWeight;
        RotateWeight(val);
    }

    private void RotateWeight(float value)
    {
        float xVal = Mathf.Clamp(90f + value, minRotation, maxRotation);
        Quaternion q = Quaternion.Euler(xVal, pillar.rotation.y, pillar.rotation.z);

        pillar.DOKill();
        pillar.DORotateQuaternion(q, 0.3f);
    }
   
}
