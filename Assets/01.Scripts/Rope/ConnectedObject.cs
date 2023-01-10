using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedObject : MonoBehaviour
{
    [SerializeField]
    private bool isFollow = true;

    [SerializeField]
    private Transform ropePosition;
    public Transform RopePosition
    {
        get
        {
            if (ropePosition) return ropePosition;
            return transform;
        }
    }

    private FollowTo follow;
    public FollowTo Follow { get => follow; }

    private WireController backWire = null;
    private WireController frontWire = null;

    public WireController StartWire { get => backWire; set => backWire = value; }
    public WireController FrontWire { get => frontWire; set => frontWire = value; }

    private void Start()
    {
        follow = Utils.GetOrAddComponent<FollowTo>(gameObject);
    }

    public void Connect(WireController wire, bool isStart)
    {
        if (follow)
        {
            if (isStart)
            {
                if (follow)
                {
                    follow.Target = wire.startAnchorTemp;
                    backWire = wire;
                }
            }
            else
            {
                if (follow)
                {
                    follow.Target = wire.endAnchorPoint;
                    frontWire = wire;
                }
            }
        }
    }

    public void UnConnect()
    {
        follow.Target = null;
    }
}