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
    Transform ropePos;

    private void Awake()
    {
        ropePos = transform.GetChild(0).transform;
    }

    private void Update()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 5f, layerMask);
        if (cols.Length <= 0) return;
        Debug.Log(cols.Length);
        ropePos = cols[0].transform;
        if (Input.GetMouseButtonDown(0))
        {
            SetConnectState();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            OnConnected();
        }
    }

    public void SetConnectState()
    {
        Debug.Log("Set_StartPosition");
        wire.AddStar(ropePos.position);
        wire.SetPosition(ropePos.position);
    }

    public void OnConnected()
    {
        Debug.Log("Connect_Rope");
        wire.AddSegment();
        wire.AddEnd();
    }


}
