using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEditor.PlayerSettings;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class RopeCollider : MonoBehaviour
{
    private const float ROPE_THICKNESS = 0.075f;
    private Transform fixedObject;
    private Transform mid;
    new private Collider collider;

    private void Start()
    {
        collider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (fixedObject && mid)
        {
            Connect(fixedObject, mid);
        }
    }

    public void Connect(Transform from, Transform to, bool isFirstConnect = false)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        if (isFirstConnect)
        {
            fixedObject = from;
            mid = to;
        }

        SetScale(from.position, mid.position);
        SetPosAndRot(from.position, mid.position);
    }

    public void UnConnect()
    {
        gameObject.SetActive(false);
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

    public void SetIsTrigger(bool isTrigger)
    {
        collider.isTrigger = isTrigger;
    }

    public void ResetRope()
    {
        Connect(fixedObject, mid);
    }
}
