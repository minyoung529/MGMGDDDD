using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Balance : MonoBehaviour
{
    [SerializeField] private BalanceFloor leftBalance;
    [SerializeField] private BalanceFloor rightBalance;

    [SerializeField] private Transform pillar;

    private Vector3 targetRotation = Vector3.zero;
    private float rotationSpeed = 2f;

    // √÷¥Î 45µµ

    private void Awake()
    {
        targetRotation = pillar.eulerAngles;
    }


    public void CompareWeight()
    {
        float val = rightBalance.GetWeight - leftBalance.GetWeight;
        RotateWeight(val);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            leftBalance.IncreaseWeight(10);
        }
        else if(Input.GetKeyDown(KeyCode.Tab))
        {
            rightBalance.IncreaseWeight(10);
        }
    }

    private void RotateWeight(float value)
    {
        float xVal = 90 + value;  // Mathf.Clamp(90f + value, 45f, 145f);
        targetRotation = new Vector3(xVal, pillar.eulerAngles.y, pillar.eulerAngles.z);
        Debug.Log(xVal);
        pillar.eulerAngles = targetRotation;

        
        //pillar.DORotate(targetRotation, 0.5f);

    }
   
}
