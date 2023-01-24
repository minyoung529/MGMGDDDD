using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class ConnectObject : MonoBehaviour
{
    [SerializeField] private Transform ropeHand;

    private Vector3 hitPoint;
    private SpringJoint joint;
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (joint)   // 연결이 된 상태
        {
            lineRenderer.SetPosition(0, ropeHand.position);
            lineRenderer.SetPosition(1, hitPoint);
        }
    }

    public void Connect(ConnectedObject connectedObj, Vector3 hitPoint, WireController wire)
    {
        lineRenderer.positionCount = 2;
        this.hitPoint = hitPoint;

        if (connectedObj)
        {

        }
        else
        {
            Swing(wire);
        }
    }

    public void UnConnect()
    {
        if (joint)
        {
            Destroy(joint);
        }

        lineRenderer.positionCount = 0;
    }

    private void Swing(WireController wire)
    {
        if (joint == null)
        {
            joint = gameObject.AddComponent<SpringJoint>();
        }

        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = hitPoint;

        // the distance grapple will try to keep from grapple point. 
        joint.maxDistance = Define.MAX_ROPE_DISTANCE;
        joint.minDistance = 1f;

        // customize values as you like
        joint.spring = 4f;
        joint.damper = 7f;
        joint.massScale = 98f;
    }
}
