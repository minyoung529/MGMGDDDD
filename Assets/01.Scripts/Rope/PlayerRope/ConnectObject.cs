using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

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
        if (ropeController.ConnectCount == 1 && !tryConnect)   // ������ �� ����
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

    public void Connect(ConnectedObject connectedObj, Vector3 point, WireController wire)
    {
        lineRenderer.positionCount = 2;

        prevHitPoint = hitPoint;
        hitPoint = point;

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
    }

    private  IEnumerator TryConnect()
    {
        tryConnect = true;

        Vector3 curPos = ropeHand.position;
        Vector3 dir = (hitPoint - ropeHand.position).normalized;

        while (true)
        {
            curPos += dir * Time.deltaTime * 35f;

            lineRenderer.SetPosition(0, curPos);

            if (Vector3.Distance(prevHitPoint, curPos) > 10f)    // Fail
                break;

            if (Vector3.Distance(curPos, hitPoint) < 1f)    // Success
            {
                SuccessConnect();
                yield break;
            }

            yield return null;
        }

        // Fail
        tryConnect = false;
        hitPoint = prevHitPoint;
        --ropeController.ConnectCount;
    }

    private IEnumerator ResetRope()
    {
        float duration = 0.4f;
        float timer = 0f;
        Vector3 prevPos, pos;
        Transform ropeEnd = ropeController.PlayerRope.endJoint.transform;
        
        lineRenderer.positionCount = 2;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            prevPos = Vector3.Lerp(prevHitPoint, ropeHand.position, timer / duration);
            pos = Vector3.Lerp(hitPoint, ropeEnd.position, timer / duration);

            lineRenderer.SetPosition(0, prevPos);
            lineRenderer.SetPosition(1, pos);

            yield return null;
        }

        ResetData();
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

        if (ropeController.ConnectCount != 0)
            StartCoroutine(ResetRope());

        connectedObjs.ForEach(x => x.UnConnect());
        connectedObjs.Clear();
    }

    private void ResetData()
    {
        ropeController.PlayerRope.Active(true);
        lineRenderer.positionCount = 0;
        prevHitPoint = Vector3.zero;
        hitPoint = Vector3.zero;
    }
}
