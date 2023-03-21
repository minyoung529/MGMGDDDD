using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balance : MonoBehaviour
{
    [SerializeField] private float minDownWeight = 0f;
    [SerializeField] private float maxDownWeight = 100f;

    [SerializeField] private BalanceFloor leftBalance;
    [SerializeField] private BalanceFloor rightBalance;

    public void CompareWeight()
    {
        if(leftBalance.GetWeight > rightBalance.GetWeight)
        {
            DownWeightFloor(leftBalance, leftBalance.GetWeight);
            UpWeightFloor(rightBalance, rightBalance.GetWeight);
        }
        else if(leftBalance.GetWeight < rightBalance.GetWeight)
        {
            DownWeightFloor(rightBalance, rightBalance.GetWeight);
            UpWeightFloor(leftBalance, leftBalance.GetWeight);
        }
    }

    public void DownWeightFloor(BalanceFloor balance, float value)
    {
    }
    public void UpWeightFloor(BalanceFloor balance, float value)
    {
    }
}
