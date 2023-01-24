using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedObject : MonoBehaviour
{
    [SerializeField]
    private bool isFollow = true;

    [SerializeField]
    private Transform ropePosition;

    private WireController frontWire = null;

    private Joint fixedJoint;
    private Rigidbody rigid;

    #region Property
    public Rigidbody Rigid => rigid;
    public Transform RopePosition
    {
        get
        {
            if (ropePosition) return ropePosition;
            else return transform;
        }
    }
    #endregion

    private void Start()
    {
        fixedJoint = GetComponent<Joint>();
        rigid = GetComponent<Rigidbody>();
    }

    public void Connect(WireController wire)
    {
        Rigidbody wireRigid = wire.endRigid;
        frontWire = wire;

        rigid.MovePosition(wireRigid.position);

        if (fixedJoint)
        {
            fixedJoint.connectedBody = wireRigid;
        }

        rigid.isKinematic = !isFollow;
    }

    public void UnConnect()
    {
        fixedJoint.connectedBody = null;
        rigid.isKinematic = true;

        // TODO: MOVE
        if (frontWire)
        {
            frontWire.endRigid.isKinematic = false;
        }
    }
}