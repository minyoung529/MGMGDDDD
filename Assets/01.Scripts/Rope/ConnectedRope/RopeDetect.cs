using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(ConnectedRope))]
public class RopeDetect : MonoBehaviour
{
    private const float ROPE_THICKNESS = 0.075f;
    private const float MAX_DIST = 2.5f;

    [SerializeField]
    private Transform mid;

    private bool isStay = false;
    private bool isCollide = false;
    private bool isForward = false;
    private Vector3 originMid;
    private Vector3 originMidFwd;

    private Transform character;
    new private BoxCollider collider;

    List<RopeCollider> ropeColliders;

    private bool GreaterThanMax => Vector3.Distance(character.position, originMid) > MAX_DIST;


    private void Start()
    {
        ropeColliders = new List<RopeCollider>(GetComponentsInChildren<RopeCollider>());
        collider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (isStay)
        {
            if (!GreaterThanMax)
            {
                Vector3 pos = character.position;
                pos += (!isForward) ? originMidFwd * 0.5f : -originMidFwd * 0.5f;
                pos.y = originMid.y + 0.5f;
                mid.position = pos;

            }

            if (isForward == IsForward())
            {
                if (Vector3.Distance(character.position, originMid) > 1.2f || !isCollide)
                {
                    ResetRope();
                }
            }
            else
            {
                ropeColliders.ForEach(x => x.SetIsTrigger(!GreaterThanMax));
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("fLY");
                Rigidbody rigid = character.GetComponent<Rigidbody>();
                Vector3 dir = (originMid - mid.transform.position).normalized;
                dir.y += 2f;
                rigid.AddForce(dir * 225f, ForceMode.Impulse);
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
            isCollide = true;

            Debug.Log(Vector3.Distance(character.position, originMid));

            isForward = Vector3.Dot(originMidFwd, (other.transform.position - originMid)) > 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == Define.PLAYER_LAYER)
        {
            isCollide = false;
            //collider.isTrigger = false;
        }
    }
    #endregion Collide

    private bool IsForward()
    {
        if (!character) return false;
        return Vector3.Dot(originMidFwd, (character.position - originMid)) > 0f;
    }

    public void SetSize()
    {
        originMid = mid.position;
        originMidFwd = mid.forward;

        Vector3 size = Vector3.one * ROPE_THICKNESS;
        size.x = ropeColliders[0].transform.localScale.z * 2;

        size.y *= 1.1f;
        size.z *= 1.1f;

        collider.size = size;
    }

    private void ResetRope()
    {
        mid.localPosition = Vector3.zero;
        ropeColliders.ForEach(x => x.ResetRope());
        isStay = false;
    }
}
