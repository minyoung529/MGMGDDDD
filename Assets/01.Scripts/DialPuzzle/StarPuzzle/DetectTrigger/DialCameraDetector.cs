using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialCameraDetector : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private bool isFront = false;

    public Action OnContact { get; set; }

    private Transform player;

    private void Start()
    {
        player = GameManager.Instance.PlayerController.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((layerMask & (1 << other.gameObject.layer)) != 0)
        {
            if (IsValidContact())
            {
                Debug.Log("ON CONTACT");
                OnContact?.Invoke();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((layerMask & (1 << collision.gameObject.layer)) != 0)
        {
            if (IsValidContact())
            {
                Debug.Log("ON CONTACT");
                OnContact?.Invoke();
            }
        }
    }

    private bool IsValidContact()
    {
        Vector3 dir = player.position - transform.position;
        float angle = Vector3.Angle(dir, transform.forward);

        bool isFrontAngle = angle < 90;

        return (isFrontAngle == isFront);
    }
}
