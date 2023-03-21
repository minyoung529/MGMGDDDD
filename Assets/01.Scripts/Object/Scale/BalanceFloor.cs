using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceFloor : MonoBehaviour
{
    private float curWeight = 0f;
    public float GetWeight { get { return curWeight; } }

    private Balance mainBalance;

    private void Awake()
    {
        mainBalance = GetComponentInParent<Balance>();
    }

    private void Start()
    {
        ResetWeight();
    }

    private void ResetWeight()
    {
        curWeight = 0;
    }

    private void IncreaseWeight(WeightObject weightObj)
    {
        if (weightObj.InWeight) return;
        weightObj.InWeight = true;

        curWeight += weightObj.GetMass;
        mainBalance.CompareWeight();

        Debug.Log("Up : " + curWeight + " " + gameObject.name);
    }
    private void DecreaseWeight(WeightObject weightObj)
    {
        if (!weightObj.InWeight) return;
        weightObj.InWeight = false;

        curWeight -= weightObj.GetMass;
        mainBalance.CompareWeight();

        Debug.Log("Down : " + curWeight + " " + gameObject.name);
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
