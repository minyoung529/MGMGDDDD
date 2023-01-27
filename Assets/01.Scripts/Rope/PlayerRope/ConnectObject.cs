using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private List<ConnectedObject> connectedObjs = new List<ConnectedObject>();
    private Vector3 prevHitPoint;

    bool tryConnect = false;
    #endregion

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        ropeController = GetComponent<RopeController>();
        connectedRope = Instantiate(connectedRope);
    }

    private void Update()
    {
        if (ropeController.ConnectCount == 1 && !tryConnect)   // 연결이 된 상태
        {
            lineRenderer.SetPosition(0, ropeHand.position);

            if (connectedObjs.Count == 1)
            {
                lineRenderer.SetPosition(1, connectedObjs.First().RopePosition.position);
            }
            else
            {
                lineRenderer.SetPosition(1, hitPoint);
            }
        }
    }

    public void Connect(ConnectedObject connectedObj, Vector3 hitPoint, WireController wire)
    {
        lineRenderer.positionCount = 2;
        this.hitPoint = hitPoint;

        if (ropeController.ConnectCount > 0)
        {
            StartCoroutine(TryConnect());
        }
        else
        {
            if (connectedObj)
            {
                connectedObj.Connect(wire, ropeController.RopeRigid);
                connectedObjs.Add(connectedObj);
            }

            Swing();
        }

        prevHitPoint = hitPoint;
    }

    private IEnumerator TryConnect()
    {
        tryConnect = true;
        float distance = 0f;
        Vector3 curPos = ropeHand.position;
        Vector3 dir = (hitPoint - ropeHand.position).normalized;

        while (distance < 5f)
        {
            Vector3 prevPos = curPos;
            curPos += dir * Time.deltaTime * 25f;
            distance = Vector3.Distance(prevPos, curPos);

            lineRenderer.SetPosition(0, curPos);

            if (Vector3.Distance(curPos, hitPoint) < 1f)
                break;

            yield return null;
        }

        if (Vector3.Distance(curPos, hitPoint) < 1f)
        {
            SuccessConnect();
        }
        else
        {
            tryConnect = false;
        }
    }

    private void Swing()
    {
        if (joint == null)
        {
            joint = gameObject.AddComponent<SpringJoint>();
        }

        joint.anchor = Vector3.zero;
        joint.autoConfigureConnectedAnchor = false;

        if (connectedObjs.Count == 0)
        {
            joint.connectedAnchor = hitPoint;
        }
        else
        {
            joint.connectedBody = connectedObjs.First().Rigid;
        }

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

    private void SuccessConnect()
    {
        ResetSwing();
        connectedRope.Connect(prevHitPoint, hitPoint, lineRenderer);
    }

    public void UnConnect()
    {
        ResetSwing();
        lineRenderer.positionCount = 0;

        connectedObjs.ForEach(x => x.UnConnect());
        connectedObjs.Clear();
    }
}
