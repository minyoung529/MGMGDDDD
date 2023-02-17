using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireAtOnce : MonoBehaviour
{
    [SerializeField] UnityEvent clear;

    private TorchLight[] torchs;

    private void Awake()
    {
        torchs = GetComponentsInChildren<TorchLight>();
    }
    public void TryLightOnClear()
    {
        StartCoroutine(AtOnceLight());
    }

    IEnumerator AtOnceLight()
    {
        yield return new WaitForSeconds(0.1f);
        bool isClear = true;

        for (int i = 0; i < torchs.Length; i++)
        {
            if (torchs[i].IsOn == false)
            {
                isClear = false;
                Failed();
                break;
            }
        }
        if (isClear)
        {
            clear.Invoke();
        }
    }

    private void Failed()
    {
        for (int i = 0; i < torchs.Length; i++)
        {
            torchs[i].OffLight();
        }
    }
}
