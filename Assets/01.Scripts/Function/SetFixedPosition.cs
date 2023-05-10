using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFixedPosition : MonoBehaviour
{
    [SerializeField]
    private Vector3 fixedLocalPos;

    private void Awake()
    {
        transform.localPosition = fixedLocalPos;
    }

    private void LateUpdate()
    {
        transform.localPosition = fixedLocalPos;
    }

    [ContextMenu("RECORD")]
    public void RecordFixedLocalPos()
    {
        fixedLocalPos = transform.localPosition;
    }

    private void OnDrawGizmosSelected()
    {
        transform.localPosition = fixedLocalPos;
    }
}
