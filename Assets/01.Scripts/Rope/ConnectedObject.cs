using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ConnectedObject : MonoBehaviour
{
    [SerializeField]
    private bool isFollow = true;
    [SerializeField]
    private bool isChild = false;
    private ConnectedObject parent;

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
        if (isChild)
        {
            FindParent();
            return;
        }

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
            parent.Connect(wire, playerRigid);
            return parent.transform;
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
        if (!joint) return;

        if (ropePosition)
        {
            joint.anchor = ropePosition.localPosition;
        }
        else if (hitPoint)
        {
            joint.anchor = hitPoint.localPosition;
        }
    }

    private void FindParent()
    {
        ConnectedObject connected = this;

        while (connected.isChild)
        {
            connected = connected.transform.parent.GetComponent<ConnectedObject>();
        }
        parent = connected;
    }
}