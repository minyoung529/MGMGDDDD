using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectOil : MonoBehaviour
{
    private int cnt = 0;

    [SerializeField]
    private UnityEvent OnEnterOil;

    [SerializeField]
    private UnityEvent OnExitOil;

    public bool IsContactOil { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.OIL_BULLET_TAG))
        {
            if (cnt++ == 0)
            {
                IsContactOil = true;
                OnEnterOil?.Invoke();
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Define.OIL_BULLET_TAG))
        {
            if (--cnt == 0)
            {
                IsContactOil = false;
                OnExitOil?.Invoke();
            }
        }
    }

}
