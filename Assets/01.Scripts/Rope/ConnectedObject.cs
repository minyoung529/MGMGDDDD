using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedObject : MonoBehaviour
{
    [SerializeField]
    private bool isFollow = true;
    [SerializeField]
    private bool isChild = false;

    [SerializeField]
    private Transform ropePosition;

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
        if (isChild) return;

        rigid = gameObject.GetOrAddComponent<Rigidbody>();

        if (!isFollow)
        {
            rigid.useGravity = false;
        }
        else
        {
            joint = GetComponent<Joint>();

            if (!joint)
            {
                joint = gameObject.AddComponent<FixedJoint>();
            }
        }
    }

    public Transform Connect(WireController wire, Rigidbody playerRigid = null)
    {
        if (isChild)
        {
            Transform connected = transform;

            while (connected.parent != null)
            {
                connected = connected.parent;
            }

            connected.GetComponent<ConnectedObject>().Connect(wire, playerRigid);
            return connected;
        }
        else
        {
            rigid.isKinematic = !isFollow;

            if (!isFollow) return transform;

            joint.connectedBody = playerRigid;
            joint.autoConfigureConnectedAnchor = false;

            SetAnchor();
        }

        return transform;
    }

    public void UnConnect()
    {
        if (!isFollow) return;

        joint.connectedBody = null;
        rigid.isKinematic = true;
    }

    public void SetAnchor()
    {
        Transform hitPoint = transform.Find("Hit Point");

        if (hitPoint)
        {
            joint.anchor = hitPoint.localPosition;
        }
    }
}