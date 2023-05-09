using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerNear : MonoBehaviour
{
    [SerializeField] Transform arriveObject;
    [SerializeField] UnityEvent onArriveEvent;

    [SerializeField] float minDistance = 10f;

    private bool isArrived = false;
    public bool IsArrived { get { return isArrived; } }

    private void Awake()
    {
        StartCoroutine(CheckNear());
    }

    private IEnumerator CheckNear()
    {
        while(true)
        {
            if (Vector3.Distance(gameObject.transform.position, arriveObject.position) <= minDistance)
            {
                if (isArrived) break;
                isArrived = true;
                onArriveEvent?.Invoke();
            }
                yield return new WaitForSeconds(0.2f);
        }
        yield return null;
    }

    public void SetStartPos(Transform targetPos)
    {

    }
}
