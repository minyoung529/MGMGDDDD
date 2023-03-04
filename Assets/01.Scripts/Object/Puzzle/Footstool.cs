using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Footstool : MonoBehaviour
{
    [SerializeField] UnityEvent footEvent;
    [SerializeField] GameObject box;
    [SerializeField] float smallTime = 0.5f;

    private void TriggerTrap()
    {
        transform.DOScaleY(0.1f, smallTime).OnComplete(()=>
        {
            Debug.Log("Clear");
            footEvent?.Invoke();
        });
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == box)
        {
            TriggerTrap();
        }
    }
}
