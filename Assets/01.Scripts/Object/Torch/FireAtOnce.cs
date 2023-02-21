using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireAtOnce : MonoBehaviour
{
    [SerializeField] UnityEvent clear;

    private Fire[] bushes;
    bool isClear = false;
    bool isTry = false;

    private void Awake()
    {
        bushes = GetComponentsInChildren<Fire>();
    }
    public void TryLightOnClear()
    {
        if (isTry || isClear) return;
        isTry= true;
        StartCoroutine(AtOnceLight());
    }

    IEnumerator AtOnceLight()
    {
        Debug.Log("Try");
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < bushes.Length; i++)
        {
            if (bushes[i].IsBurn == false)
            {
                isClear = false;
                Failed();
                isTry= false;
                break;
            }
            else
            {
                isClear = true;
            }
        }
        if (isClear)
        {
            Debug.Log("Clear");
            clear.Invoke();
        }
                isTry= false;
    }

    private void Failed()
    {
        for (int i = 0; i < bushes.Length; i++)
        {
            Debug.Log("Failed");
            bushes[i].StopBurn();
        }
    }


}
