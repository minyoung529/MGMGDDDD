using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GearItem : Item
{
    [SerializeField] UnityEvent useItem;

    private bool isNearEquipPoint = false;
    private string quipPoint = Define.EQUIP_POINT;

    public override void UseItem()
    {
        base.UseItem();

        if(isNearEquipPoint)
        {
            Debug.Log("Clear");
            useItem.Invoke();
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.CompareTag(quipPoint))
        {
            isNearEquipPoint = true;
        }
    }
    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.CompareTag(quipPoint))
        {
            isNearEquipPoint = false;
        }
    }

}
