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

    #region Property
    public bool IsInstalled => isInstalled;
    #endregion

    void Awake()
    {
        collider = GetComponent<Collider>();
        rigid = GetComponent<Rigidbody>();
        respawnObject = GetComponent<RespawnObject>();
    }

    void Update()
    {
        if (rigid && isInstalled)
        {
            rigid.constraints = RigidbodyConstraints.FreezeAll;
            Debug.Log("FREEZEALL");
        }
    }

    public void Installed()
    {
        isInstalled = true;
    }

    public void UnInstalled()
    {
        isInstalled = false;
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
