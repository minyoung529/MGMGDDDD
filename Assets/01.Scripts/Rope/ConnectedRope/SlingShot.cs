using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(ConnectedRope))]
public class SlingShot : MonoBehaviour
{
    private ConnectedRope connectedRope;

    private bool isStay = false;
    private bool isForward = false;
    private Vector3 originMid;
    private Vector3 originMidFwd;

    private Transform character;
    new private BoxCollider collider;

    private SpringJoint joint;

    private void Start()
    {
        collider = GetComponent<BoxCollider>();
        connectedRope = GetComponent<ConnectedRope>();
        connectedRope.OnConnect += Connect;
    }

    private void Update()
    {
        if (isStay)
        {
            CalculateMid();

            if (isForward == IsForward())
            {
                if (Vector3.Distance(character.position, originMid) > 1.2f)
                {
                    ResetRope();
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Fly();
            }
        }
    }

    #region Collide
    private void OnTriggerEnter(Collider other)
    {
        if (!isStay && other.gameObject.layer == Define.PLAYER_LAYER)
        {
            character = other.gameObject.transform;
            isStay = true;
            isForward = Vector3.Dot(originMidFwd, (other.transform.position - originMid)) > 0f;
            SetSpringJoint();
        }
    }
    #endregion Collide

    public void Connect()
    {
        originMid = connectedRope.Mid;
        originMidFwd = transform.forward;
    }

    private void Fly()
    {
        Rigidbody rigid = character.GetComponent<Rigidbody>();
        Vector3 dir = (originMid - connectedRope.Mid + Vector3.up * 15f).normalized;
        rigid.AddForce(dir * 770f, ForceMode.Impulse);
    }

    private void SetSpringJoint()
    {
        if (!character) return;

        joint = character.GetOrAddComponent<SpringJoint>();

        joint.connectedAnchor = Vector3.zero;
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = originMid;

        // the distance grapple will try to keep from grapple point. 
        joint.maxDistance = 2f;//Define.MAX_ROPE_DISTANCE;
        joint.minDistance = 0f;//0.01f;

        // customize values as you like
        joint.spring = 50f;
        joint.damper = 20f;
        joint.massScale = 98f;
    }

    #region
    private void CalculateMid()
    {
        Vector3 pos = character.position;
        pos += (!isForward) ? originMidFwd * 0.5f : -originMidFwd * 0.5f;
        pos.y = originMid.y + 0.2f;
        connectedRope.Mid = pos;
    }

    private bool IsForward()
    {
        if (!character) return false;
        return Vector3.Dot(originMidFwd, (character.position - originMid)) > 0f;
    }

    private void ResetRope()
    {
        character = null;
        collider.isTrigger= false;
        connectedRope.Mid = originMid;
        isStay = false;

        Destroy(joint);
    }
    #endregion
}
