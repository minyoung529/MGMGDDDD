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

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            // 나중에 고쳐야 함...
            FreeLookCameraHolder holder = other.GetComponentInChildren<FreeLookCameraHolder>();

            if (!holder) return;

            if (isOriginalOrbit)
            {
                holder.SetCameraRigOriginal();
            }
            else
            {
                holder.ChangeCameraRig(orbits);
            }
        }
    }
}
