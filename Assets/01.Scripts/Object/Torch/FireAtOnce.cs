using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireAtOnce : MonoBehaviour
{
    [SerializeField] UnityEvent clear;

    private BushObject[] bushes;

    private void Awake()
    {
        bushes = GetComponentsInChildren<BushObject>();
    }
    public void TryLightOnClear()
    {
        StartCoroutine(AtOnceLight());
    }

    IEnumerator AtOnceLight()
    {
        yield return new WaitForSeconds(0.1f);
        bool isClear = true;

        for (int i = 0; i < bushes.Length; i++)
        {
            if (bushes[i].IsBurn == false)
            {
                isClear = false;
                Failed();
                break;
            }
        }
        if (isClear)
        {
            Debug.Log("Clear");
            clear.Invoke();
        }
    }

    private void Failed()
    {
        for (int i = 0; i < bushes.Length; i++)
        {
            Debug.Log("Failed");
            bushes[i].OffBurn();
        }
    }
}
