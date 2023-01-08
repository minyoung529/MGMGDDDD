using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeTarget : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    public Transform Target { get { return target; } set { target = value; } }

    [SerializeField]
    private Transform movedTransform;

    private void Update()
    {
        movedTransform.position = target.position;
    }
}
