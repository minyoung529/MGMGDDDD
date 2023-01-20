using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class RopeCollider : MonoBehaviour
{
    private const float ROPE_THICKNESS = 0.075f;
    private Transform fixedObject;
    private Vector3 originalMid;
    private Transform character;

    private bool isStay = false;

    private void Update()
    {
        if (isStay)
        {

        }
    }

    public void Connect(Transform fixedObj, Vector3 to, bool isFirstConnect = false)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        if (isFirstConnect)
        {
            fixedObject = fixedObj;
            originalMid = to;
        }

        SetScale(fixedObj.position, to);
        SetPosAndRot(fixedObj.position, to);
    }

    public void UnConnect()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == Define.PLAYER_LAYER)
        {
            character = collision.transform;
            isStay = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == Define.PLAYER_LAYER)
        {
            Vector3 pos = collision.transform.position;
            pos.y = (fixedObject.position.y + originalMid.y) * 0.5f;

            Connect(fixedObject, pos);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == Define.PLAYER_LAYER)
        {
            isStay = false;
            Connect(fixedObject, originalMid);
        }
    }

    private void SetScale(Vector3 from, Vector3 to)
    {
        Vector3 scale = Vector3.one * ROPE_THICKNESS;
        scale.z = Vector3.Distance(from, to);
        transform.localScale = scale;
    }

    private void SetPosAndRot(Vector3 from, Vector3 to)
    {
        float distance = Vector3.Distance(from, to);
        Vector3 dir = (to - from).normalized;

        transform.position = from + dir * distance * 0.5f;
        transform.forward = dir;
    }
}
