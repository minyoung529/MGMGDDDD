using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedObject : MonoBehaviour
{
    [SerializeField]
    private bool isFollow = true;

    private WireController frontWire = null;

    private Joint fixedJoint;
    private Rigidbody rigid;

    #region Property
    public Rigidbody Rigid => rigid;
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
        Rigidbody wireRigid = wire.startRigid;

        if (!isStart)
        // ---O
        {
            frontWire = wire;
            wireRigid = wire.endRigid;
        }

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

        if (frontWire)
        {
            frontWire.endRigid.isKinematic = false;
        }
    }
}