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

    private LayerMask oilLayer;

    void Start()
    {
        oilLayer = LayerMask.GetMask("Ignore Raycast");
    }

    void Update()
    {
        if (IsContactOil)
        {
            if (!IsDetectOil())
            {
                IsContactOil = false;
                OnExitOil?.Invoke();
            }
        }
        else
        {
            if (IsDetectOil())
            {
                IsContactOil = true;
                OnEnterOil?.Invoke();
            }
        }
    }

    private bool IsDetectOil()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 3f, oilLayer);

        foreach (Collider col in cols)
        {
            if (col.CompareTag(Define.OIL_BULLET_TAG))
            {
                return true;
            }
        }

        return false;
    }
}