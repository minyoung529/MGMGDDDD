using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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

    [SerializeField]
    private float flyForce = 5f;

    private void Awake()
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
                Vector3 characterpos = character.position;
                Vector3 mid = originMid;

                characterpos.y = mid.y = 0f;

                if (Vector3.Distance(characterpos, mid) > 1f)
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
        float dist = Vector3.Distance(connectedRope.Mid, originMid);
        Vector3 dir = (originMid - connectedRope.Mid).normalized;
        dir.y = 0f;

        dist = Mathf.Clamp(dist * 10f, 3f, 30f);

        Vector3 velocity = GetVelocity(character.transform.position, character.transform.position + dir * dist, 70f);
        rigid.velocity = velocity;
        //Vector3[] path =
        //{
        //    character.position, 
        //    character.position + character.forward * 5f + Vector3.up * 10f,
        //    character.position + character.forward * 10f
        //};

        //character.transform.DOPath(path, 2f, PathType.CubicBezier, PathMode.Full3D);

        Destroy(joint);
    }

    private void SetSpringJoint()
    {
        if (!character) return;

        Debug.Log("ADD");
        joint = character.GetOrAddComponent<SpringJoint>();

        joint.anchor = Vector3.zero;
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

    #region Calculate
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

    Vector3 GetVelocity(Vector3 currentPos, Vector3 targetPos, float initialAngle)
    {
        float gravity = Physics.gravity.magnitude;
        float angle = initialAngle * Mathf.Deg2Rad;

        Vector3 planarTarget = new Vector3(targetPos.x, 0, targetPos.z);
        Vector3 planarPosition = new Vector3(currentPos.x, 0, currentPos.z);

        float distance = Vector3.Distance(planarTarget, planarPosition);
        float yOffset = currentPos.y - targetPos.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPosition) * (targetPos.x > currentPos.x ? 1 : -1);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        return finalVelocity;
    }
    #endregion

    private void ResetRope()
    {
        Debug.Log("Reset");

        ConnectedRope.IsSlingShot = false;
        character = null;
        collider.isTrigger = false;
        connectedRope.Mid = originMid;
        isStay = false;

        Destroy(joint);
    }
}
