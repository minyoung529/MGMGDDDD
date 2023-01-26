using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class ConnectObject : MonoBehaviour
{
    [SerializeField] private Transform ropeHand;
    private RopeController ropeController;
    #region Swing Variable
    private Vector3 hitPoint;
    private SpringJoint joint;
    private LineRenderer lineRenderer;
    #endregion

    #region Connect Object
    [SerializeField] private ConnectedRope connectedRope;
    private ConnectObject prevConnectedObj;
    private Vector3 prevHitPoint;
    #endregion

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        ropeController = GetComponent<RopeController>();
        connectedRope = Instantiate(connectedRope);
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

        if (ropeController.ConnectCount > 0)
        {
            ResetSwing();

            //if (connectedObj)
            //{
            //connectedRope.Connect(connectedObj.RopePosition.position, hitPoint, lineRenderer);
            //}
            //else
            //{
            connectedRope.Connect(prevHitPoint, hitPoint, lineRenderer);
            //}
        }
        else
        {
            Swing();
        }

        prevHitPoint = hitPoint;
    }

    private void Swing()
    {
        if (joint == null)
        {
            joint = gameObject.AddComponent<SpringJoint>();
        }

        joint.anchor = Vector3.zero;
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = hitPoint;

        // the distance grapple will try to keep from grapple point. 
        joint.maxDistance = Define.MAX_ROPE_DISTANCE;
        joint.minDistance = 0f;

        // customize values as you like
        joint.spring = 30f;
        joint.damper = 10f;

        joint.massScale = 98f;
    }

    private void ResetSwing()
    {
        if (joint)
        {
            Destroy(joint);
        }
    }

    public void UnConnect()
    {
        ResetSwing();
        lineRenderer.positionCount = 0;
    }
}
