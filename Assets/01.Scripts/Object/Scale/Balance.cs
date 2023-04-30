using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balance : MonoBehaviour
{
    [SerializeField] private float maxRotation = 40f;
    [SerializeField] private float rotationTime = 0.7f;

    [SerializeField] private BalanceFloor leftBalance;
    [SerializeField] private BalanceFloor rightBalance;

    [SerializeField] private Transform leftPillar;
    [SerializeField] private Transform rightPillar;
    
    private Transform leftAnchor;
    private Transform rightAnchor;
    private Transform pillar;

    private void Awake()
    {
        pillar = transform.GetChild(0);
        leftAnchor = pillar.GetChild(0);
        rightAnchor = pillar.GetChild(1);
    }
    private void Start()
    {
        CompareWeight();
    }
    private void FixedUpdate()
    {
        leftPillar.position = leftAnchor.position;
        rightPillar.position = rightAnchor.position;
    }

    public void CompareWeight()
    {
        float val = rightBalance.GetWeight - leftBalance.GetWeight;

        if (val > maxRotation) val = maxRotation;
        else if(val < 0) val = 0;
        Debug.Log(val);

        RotateWeight(val);
    }

    private void RotateWeight(float value)
    {
        Quaternion origin = Quaternion.Euler(pillar.rotation.x, pillar.rotation.y, value);
        Quaternion q = Quaternion.Euler(pillar.rotation.x, pillar.rotation.y, value - 2f);

        pillar.DOKill();
        pillar.DOLocalRotateQuaternion(q, rotationTime).OnComplete(()=>
        {
            pillar.DOLocalRotateQuaternion(origin, rotationTime);
        });
    }
   
}
