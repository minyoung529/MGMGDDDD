using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeObject : MonoBehaviour
{
    private bool isInstalled = false;

    private Rigidbody rigid;
    new private Collider collider;
    private RespawnObject respawnObject;
    private SlidingObject sliding;

    #region Property
    public bool IsInstalled => isInstalled;
    #endregion

    void Awake()
    {
        collider = GetComponent<Collider>();
        rigid = GetComponent<Rigidbody>();
        respawnObject = GetComponent<RespawnObject>();
        sliding = GetComponent<SlidingObject>();
    }

    void Update()
    {
        if (rigid && isInstalled)
        {
            rigid.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public void Installed()
    {
        isInstalled = true;
        sliding.enabled = false;
    }

    public void UnInstalled()
    {
        isInstalled = false;
        sliding.enabled = true;
    }

    public void Respawn()
    {
        UnInstalled();
        respawnObject.Respawn();
        SetConstraints(RigidbodyConstraints.FreezeAll ^ RigidbodyConstraints.FreezePositionY);
    }

    public void SetConstraints(RigidbodyConstraints constraints)
    {
        rigid.constraints = constraints;
    }
}
