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

    private Joint joint;
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
        rigid = gameObject.GetOrAddComponent<Rigidbody>();

        if (!isFollow)
            rigid.useGravity = false;

        if (isFollow)
        {
            joint = GetComponent<Joint>();

            if (!joint)
            {
                joint = gameObject.AddComponent<FixedJoint>();
            }
        }
    }

    public void Connect(WireController wire, Rigidbody playerRigid = null)
    {
        rigid.isKinematic = !isFollow;

        if (!isFollow) return;

        if (gameObject.layer == Define.PET_LAYER)
        {
            Rigidbody wireRigid = wire.endRigid;
            frontWire = wire;

            rigid.MovePosition(wireRigid.position);

            joint.connectedBody = wireRigid;
        }
        else
        {
            joint.connectedBody = playerRigid;
        }
    }

    public void UnConnect()
    {
        if (!isFollow) return;

        joint.connectedBody = null;
        rigid.isKinematic = true;
    }
}