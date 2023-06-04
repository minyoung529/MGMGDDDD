using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BalanceFloor : MonoBehaviour
{
    [SerializeField] private float curWeight = 0f;
    public float GetWeight { get { return curWeight; } }

    private Balance mainBalance;

    [SerializeField]
    private UnityEvent onIncrease;
    [SerializeField]
    private UnityEvent onDecrease;

    private void Awake()
    {
        mainBalance = GetComponentInParent<Balance>();
    }

    public void EnterBalanceFloor(WeightObject weightObj)
    {
        weightObj.EnterFloor(this);
    }

    public void IncreaseWeight(float value)
    {
        curWeight += value;
        mainBalance.CompareWeight();

        onIncrease?.Invoke();
    }

    public void ExitBalanceFloor(WeightObject weightObj)
    {
        if (!weightObj.InWeight) return;
        weightObj.ExitFloor(this);

        DecreaseWeight(weightObj.GetMass);
    }
    public void DecreaseWeight(float value)
    {
        curWeight -= value;
        if (curWeight < 0)
        {
            curWeight = 0;
        }
        mainBalance.CompareWeight();
        onDecrease?.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        WeightObject weightObject = other.GetComponent<WeightObject>();
        if (weightObject != null) EnterBalanceFloor(weightObject);
    }

    private void OnTriggerExit(Collider other)
    {
        WeightObject weightObject = other.GetComponent<WeightObject>();
        if (weightObject != null) ExitBalanceFloor(weightObject);
    }
}
