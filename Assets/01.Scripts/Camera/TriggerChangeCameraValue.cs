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

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            // 나중에 고쳐야 함...
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
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            // 나중에 고쳐야 함...
            FreeLookCameraHolder holder = other.GetComponentInChildren<FreeLookCameraHolder>();

            if (!holder) return;

            holder.SetCameraRigOriginal(duration);
        }
    }
}
