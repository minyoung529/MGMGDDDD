using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RopeConnect : MonoBehaviour
{
    [SerializeField]
    LayerMask layerMask;
    [SerializeField]
    WireController wire;

    WireController wireCtrl;
    Transform ropePos;

    private void Awake()
    {
        ropePos = transform.GetChild(0).transform;
    }

    private void Update()
    {

    }

    public void OnConnect()
    {
        Debug.Log("On_StartPosition");

        WireController w = Instantiate(wire.gameObject, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<WireController>();
        w.AddStart(ropePos.position);
        w.SetPosition(ropePos.position);
        w.AddSegment();
    }

    public void ThrowRope()
    {
        Vector3 wirePos = new Vector3(transform.position.x, 0, transform.position.z);

        wireCtrl.AddStart(transform.position);
        wireCtrl.SetPosition(gameObject.transform.forward * 10f);

        wireCtrl.AddSegment();
        wireCtrl.AddEnd();
    }

}
