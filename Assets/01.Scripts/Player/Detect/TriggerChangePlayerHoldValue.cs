using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerChangePlayerHoldValue : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;

    [Header("Set Value")]
    [SerializeField] private float setPower;

    [SerializeField] private float setAngle;

    [SerializeField] private bool once = false;
    private bool isEnter = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isEnter) return;

        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            isEnter = true;
            PlayerHold holder = other.GetComponentInChildren<PlayerHold>();

            if (!holder) return;
            
            holder.SetThrowAngle(setAngle);
            holder.SetThrowPower(setPower);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isEnter) return;

        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            if (!once) isEnter = false;
            PlayerHold holder = other.GetComponentInChildren<PlayerHold>();

            if (!holder) return;

            holder.SetDefault();
        }
    }
}
