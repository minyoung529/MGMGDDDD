using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedObject : MonoBehaviour
{
    [SerializeField]
    private bool isFollow = true;

    [SerializeField]
    private Transform ropePosition;

    private WireController backWire = null;
    private WireController frontWire = null;

    private Joint fixedJoint;
    private Rigidbody rigid;

    #region Property
    public Rigidbody Rigidbody => rigid;
    public WireController StartWire { get => backWire; set => backWire = value; }
    public WireController FrontWire { get => frontWire; set => frontWire = value; }
    public Transform RopePosition
    {
        get
        {
            if (ropePosition) return ropePosition;
            return transform;
        }
    }
    #endregion

    private void Start()
    {
        fixedJoint = GetComponent<Joint>();
        rigid = GetComponent<Rigidbody>();
    }

    public void Connect(WireController wire, bool isStart)
    {
        if (isStart)
        {
            // O---
            if (isFollow)
            {
                //ropePosition.position = wire.startRigid.position;
                //wire.startRigid.isKinematic = true;
                //rigid.isKinematic = false;

                //if (fixedJoint)
                //    fixedJoint.connectedBody = wire.startRigid;
            }

            backWire = wire;
        }
        // ---O
        else
        {
            if (isFollow)
            {
                rigid.isKinematic = false;
            }
            else
            {
                rigid.isKinematic = true;
                wire.endRigid.isKinematic = true;
            }

            ropePosition.position = wire.endRigid.position;

            if (fixedJoint)
                fixedJoint.connectedBody = wire.endRigid;

            frontWire = wire;
        }
    }

    public void UnConnect()
    {
        fixedJoint.connectedBody = null;
        rigid.isKinematic = true;

        frontWire.endRigid.isKinematic = false;
    }
}