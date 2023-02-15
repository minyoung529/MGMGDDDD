using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GetDark : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private float duration = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            RenderSettingController.SetReflectionIntensity(0f, duration);
            RenderSettingController.SetAmbiendLight((Color)new Color32(60, 60, 60, 255), duration);
            RenderSettingController.SetFogColor(Color.black, duration);
            RenderSettingController.SetFogDensity(0.06f, duration);
        }
    }
}
