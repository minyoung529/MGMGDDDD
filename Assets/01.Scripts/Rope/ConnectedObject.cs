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
        fixedJoint = GetComponent<FixedJoint>();
        rigid = GetComponent<Rigidbody>();
    }

    public void Connect(WireController wire, bool isStart)
    {
        if (isStart)
        {
            if (isFollow)
            {
                ropePosition.position = wire.startRigid.position;
                wire.startRigid.isKinematic = true;
                rigid.isKinematic = false;

                if (fixedJoint)
                    fixedJoint.connectedBody = wire.startRigid;
            }

            backWire = wire;
        }
        else
        {
            if (isFollow)
            {
                ropePosition.position = wire.endRigid.position;
                wire.endRigid.isKinematic = true;
                rigid.isKinematic = false;

                if (fixedJoint)
                    fixedJoint.connectedBody = wire.endRigid;
            }

            frontWire = wire;
        }
    }

    public void UnConnect()
    {
        fixedJoint.connectedBody = null;
        rigid.isKinematic = true;
    }
}