using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceFloor : MonoBehaviour
{
    [SerializeField] private float curWeight = 0f;
    public float GetWeight { get { return curWeight; } }

    private Balance mainBalance;

    private void Awake()
    {
        mainBalance = GetComponentInParent<Balance>();
    }

    public void IncreaseWeight(WeightObject weightObj)
    {
        if (weightObj.InWeight) return;
        weightObj.InWeight = true;

        curWeight += weightObj.GetMass;
        
        mainBalance.CompareWeight();
    }

    public void IncreaseWeight(float value)
    {
        curWeight += value;
        
        mainBalance.CompareWeight();
    }

    public void DecreaseWeight(WeightObject weightObj)
    {
        if (!weightObj.InWeight) return;
        weightObj.InWeight = false;

        curWeight -= weightObj.GetMass;
        if (curWeight < 0)
        {
            curWeight = 0;
        }
        mainBalance.CompareWeight();
    }
    public void DecreaseWeight(float value)
    {
        curWeight -= value;
        if (curWeight < 0)
        {
            curWeight = 0;
        }
        mainBalance.CompareWeight();
    }

    private void OnTriggerEnter(Collider other)
    {
        WeightObject weightObject = other.GetComponent<WeightObject>();
        if (weightObject != null) IncreaseWeight(weightObject);
    }

    private void OnTriggerExit(Collider other)
    {
        WeightObject weightObject = other.GetComponent<WeightObject>();
        if (weightObject != null) DecreaseWeight(weightObject);
    }
}
