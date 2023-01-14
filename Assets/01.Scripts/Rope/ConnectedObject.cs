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
    public Rigidbody Rigid => rigid;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            rigid.MovePosition(transform.position - transform.forward * 0.5f);
        }
    }

    public void Connect(WireController wire, bool isStart)
    {
        Rigidbody wireRigid;

        if (isStart)
        {
            backWire = wire;
            wireRigid = wire.startRigid;
        }
        // ---O
        else
        {
            frontWire = wire;
            wireRigid = wire.endRigid;
        }

        rigid.MovePosition(wireRigid.position);

        if (fixedJoint)
        {
            fixedJoint.connectedBody = wireRigid;
        }

        if (isFollow)
        {
            rigid.isKinematic = false;
        }
        else
        {
            rigid.isKinematic = true;
            wireRigid.isKinematic = true;
        }
    }

    public void UnConnect()
    {
        fixedJoint.connectedBody = null;
        rigid.isKinematic = true;

        frontWire.endRigid.isKinematic = false;
    }
}