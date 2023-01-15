using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTo : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    public Transform Target { get => target; set => target = value; }

    private Transform movedTransform;
    public Transform MovedTarget { get => target; set => target = value; }

    void Start()
    {
        if (!movedTransform)
            movedTransform = transform;
    }

    void Update()
    {
        if (target)
        {
            Vector3 direction =  target.position - movedTransform.position;
            movedTransform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }
}
