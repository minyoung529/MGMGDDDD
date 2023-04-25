using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerChangeCameraValue : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private CinemachineFreeLook.Orbit[] orbits;

    [SerializeField]
    private bool isOriginalOrbit = false;

    [SerializeField]
    private float duration = 1f;

    private bool isEnter = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isEnter) return;

        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            isEnter = true;
            // ���߿� ���ľ� ��...
            FreeLookCameraHolder holder = other.GetComponentInChildren<FreeLookCameraHolder>();

            if (!holder) return;

            if (isOriginalOrbit)
            {
                holder.SetCameraRigOriginal(duration);
            }
            else
            {
                holder.ChangeCameraRig(orbits, duration);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isEnter) return;

        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            isEnter = false;
            // ���߿� ���ľ� ��...
            FreeLookCameraHolder holder = other.GetComponentInChildren<FreeLookCameraHolder>();

            if (!holder) return;

            holder.SetCameraRigOriginal(duration);
        }
    }
}
